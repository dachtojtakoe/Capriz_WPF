using System;
using System.IO.Ports;

namespace Capriz_WPF.Common
{
    public class PIConfiguratorProtocol : IDisposable
    {
        public const int SetSize = 28;
        public const int GetSize = 7;
        public const int ResponseSize = 28;
        public const int PortsCount = 7;

        private SerialPort _port;
        private readonly byte[] _buf = new byte[ResponseSize];
        private int _idx;
        private bool _reading;

        public event Action<byte[]> PacketReceived;
        public event Action<string> Error;

        public bool IsOpen => _port != null && _port.IsOpen;

        public void Open(string portName, int baudRate)
        {
            Close();
            _port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 300,
                WriteTimeout = 500,
                DtrEnable = false,
                RtsEnable = false
            };
            _port.DataReceived += OnDataReceived;
            _port.Open();
            _idx = 0;
            _reading = false;
        }

        public void Close()
        {
            try
            {
                if (_port != null)
                {
                    _port.DataReceived -= OnDataReceived;
                    if (_port.IsOpen) _port.Close();
                    _port.Dispose();
                    _port = null;
                }
            }
            catch { }
            _idx = 0;
            _reading = false;
        }

        // ---------- Сборка сообщения ----------

        /// <summary>
        /// Собирает 28-байтный $SET.
        /// </summary>
        /// <param name="windTypeIndex">0 = истинный, 1 = кажущийся</param>
        /// <param name="averagingIndex">0 = мгновенные, 1 = 2 мин, 2 = 10 мин</param>
        /// <param name="baudPerPort">6 элементов на порт: [0..6] — индексы ComboBox</param>
        /// <param name="rxPerPort">7 элементов: 0..5 (0 = ничего, 1..5 = VTG/VHW/RMC/VBW/HDT)</param>
        /// <param name="txPerPort">7 элементов: битовая маска 0..15 (ALB=1, MWV=2, XDR=4, MWD=8)</param>
        public static byte[] BuildSet(int windTypeIndex, int averagingIndex,
                                      int[] baudPerPort, int[] rxPerPort, int[] txPerPort)
        {
            if (baudPerPort == null || baudPerPort.Length != PortsCount) throw new ArgumentException(nameof(baudPerPort));
            if (rxPerPort == null || rxPerPort.Length != PortsCount) throw new ArgumentException(nameof(rxPerPort));
            if (txPerPort == null || txPerPort.Length != PortsCount) throw new ArgumentException(nameof(txPerPort));

            var b = new byte[SetSize];
            b[0] = (byte)'$';
            b[1] = (byte)'S';
            b[2] = (byte)'E';
            b[3] = (byte)'T';

            // Байт 5: ветер + осреднение
            int windAvg = 48 + averagingIndex + (windTypeIndex == 0 ? 3 : 0);
            b[4] = (byte)windAvg;

            // Байты 6..26: 7 портов × 3 байта (baud, rx, tx)
            int pos = 5;
            for (int i = 0; i < PortsCount; i++)
            {
                b[pos++] = (byte)(48 + baudPerPort[i]);            // '0'..'5'
                b[pos++] = (byte)(48 + rxPerPort[i]);              // '0'..'5'
                b[pos++] = (byte)(64 + (txPerPort[i] & 0x0F));     // '@'..'O'
            }

            // Байты 27..28: CR LF
            b[26] = 0x0D;
            b[27] = 0x0A;
            return b;
        }

        public static byte[] BuildGet()
        {
            return new byte[] { (byte)'$', (byte)'G', (byte)'E', (byte)'T',
                                (byte)'C', 0x0D, 0x0A };
        }

        // ---------- Парсинг ответа ----------

        public struct PacketData
        {
            public int WindTypeIndex;      // 0 = истинный, 1 = кажущийся
            public int AveragingIndex;     // 0..2
            public int[] BaudPerPort;      // 0..5
            public int[] RxPerPort;        // 0..5
            public int[] TxPerPort;        // 0..15
        }

        /// <summary>
        /// Разбирает 28-байтный $PRG-пакет.
        /// </summary>
        public static bool TryParseResponse(byte[] p, out PacketData data)
        {
            data = default;
            if (p == null || p.Length < ResponseSize) return false;
            if (p[0] != (byte)'$' || p[1] != (byte)'P' || p[2] != (byte)'R' || p[3] != (byte)'G')
                return false;

            int v = p[4]; // '0'..'5'
            data.AveragingIndex = v % 3;
            data.WindTypeIndex = (v - 48) >= 3 ? 0 : 1;
            // 51,52,53 → истинный (0); 48,49,50 → кажущийся (1)

            data.BaudPerPort = new int[PortsCount];
            data.RxPerPort = new int[PortsCount];
            data.TxPerPort = new int[PortsCount];

            int pos = 5;
            for (int i = 0; i < PortsCount; i++)
            {
                data.BaudPerPort[i] = p[pos++] - 48;        // '0'..'5'
                data.RxPerPort[i] = p[pos++] - 48;        // '0'..'5'
                data.TxPerPort[i] = p[pos++] - 64;        // '@'..'O' → 0..15
            }
            return true;
        }

        // ---------- Отправка ----------

        public void SendSet(byte[] setPacket)
        {
            try
            {
                if (!IsOpen) { Error?.Invoke("Порт не открыт"); return; }
                _port.DiscardInBuffer();
                _idx = 0;
                _reading = false;
                _port.Write(setPacket, 0, setPacket.Length);
            }
            catch (Exception ex) { Error?.Invoke(ex.Message); }
        }

        public void SendGet()
        {
            SendSet(BuildGet());
        }

        // ---------- Приём ----------

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int toRead = _port.BytesToRead;
                for (int i = 0; i < toRead; i++)
                {
                    byte b = (byte)_port.ReadByte();

                    if (!_reading)
                    {
                        if (b == (byte)'$')
                        {
                            _idx = 0;
                            _buf[_idx++] = b;
                            _reading = true;
                        }
                        continue;
                    }

                    if (_idx >= ResponseSize) { _reading = false; _idx = 0; continue; }
                    _buf[_idx++] = b;

                    if (_idx == ResponseSize)
                    {
                        if (_buf[0] == (byte)'$' && _buf[1] == (byte)'P' &&
                            _buf[2] == (byte)'R' && _buf[3] == (byte)'G')
                        {
                            var copy = new byte[ResponseSize];
                            Array.Copy(_buf, copy, ResponseSize);
                            PacketReceived?.Invoke(copy);
                        }
                        _reading = false;
                        _idx = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _reading = false;
                _idx = 0;
                Error?.Invoke(ex.Message);
            }
        }

        public void Dispose() => Close();
    }
}