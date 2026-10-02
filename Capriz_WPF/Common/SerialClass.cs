using Capriz_WPF.Data;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Capriz_WPF.Common
{
    public class SerialClass : SerialPort
    {
        private const int FrameSize = 405;
        private readonly byte[] _buf = new byte[FrameSize];
        private int _idx;
        private bool _reading;


        private const int ConfigResponseSize = 28;   // было FrameSize = 405
        private int _expectedSize = FrameSize;       // сколько читать в текущем кадре

        private readonly object _sendLock = new object();
        private byte[] _pendingSend;

        public event Action<byte[]> ConfigPacketReceived;

        public SerialClass(string portName = "COM1", int baudRate = 9600, int dataBits = 8,
            StopBits stopBits = StopBits.One, Parity parity = Parity.None, Handshake handshake = Handshake.None)
        {
            PortName = portName;
            BaudRate = baudRate;
            DataBits = dataBits;
            StopBits = stopBits;
            Parity = parity;
            Handshake = handshake;
            ReadTimeout = 500;
            WriteTimeout = 500;
            DataReceived += DataReceive;
        }

        public string[] GetSystemPorts() =>
            SerialPort.GetPortNames()
                .OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out int n) ? n : 0)
                .ToArray();

        public byte OpenPort(string portName = "COM1", int baudRate = 9600, int dataBits = 8,
            StopBits stopBits = StopBits.One, Parity parity = Parity.None, Handshake handshake = Handshake.None)
        {
            if (IsOpen) Close();
            try
            {
                PortName = portName; BaudRate = baudRate; DataBits = dataBits;
                Parity = parity; StopBits = stopBits; Handshake = handshake;
                Open();
                DiscardOutBuffer();
                DiscardInBuffer();
                DataDelegates.EventHandlerStr(portName + " is open." + Environment.NewLine);
                return 1;
            }
            catch (Exception err)
            {
                DataDelegates.EventHandlerStr(err.Message + Environment.NewLine);
                return 0;
            }
        }

        public void ClosePort()
        {
            try
            {
                if (IsOpen)
                {
                    Close();
                    DataDelegates.EventHandlerStr(PortName + " is close." + Environment.NewLine);
                }
            }
            catch (Exception ex) { DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine); }
        }

        void DataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;
            try
            {
                int toRead = port.BytesToRead;
                for (int i = 0; i < toRead; i++)
                {
                    byte b = (byte)port.ReadByte();

                    if (!_reading)
                    {
                        // ищем начало кадра — одиночный '$'
                        if (b == (byte)'$')
                        {
                            _idx = 0;
                            _buf[_idx++] = b;
                            _reading = true;
                            _expectedSize = 0; // ещё не знаем
                        }
                        continue;
                    }

                    // защита от переполнения — сбрасываем кадр
                    if (_idx >= FrameSize)
                    {
                        _reading = false;
                        _idx = 0;
                        continue;
                    }

                    _buf[_idx++] = b;
                    //DataDelegates.EventHandlerStr("" + _idx + Environment.NewLine);

                    //    // кадр собран целиком?
                    //    if (_idx == FrameSize)
                    //    {

                    //        if (_buf[FrameSize - 2] == 0x0D && _buf[FrameSize - 1] == 0x0A)
                    //        {
                    //            var message = ConvertData.GetMessage(_buf);
                    //            if (!string.IsNullOrEmpty(message))
                    //            {
                    //                DataDelegates.EventHandlerStr(message);
                    //                DataDelegates.WriteFHandlerStr(message);
                    //                DataDelegates.EventHandlerStrParam(message);
                    //            }
                    //            else
                    //            {
                    //                //DataDelegates.EventHandlerStr(_buf + Environment.NewLine);
                    //                DataDelegates.EventHandlerStr("CRC error" + Environment.NewLine);
                    //            }
                    //        }
                    //        _reading = false;
                    //        _idx = 0;
                    //    }
                    //}

                    if (_idx == 4 && _expectedSize == 0)
                    {
                        if (_buf[1] == (byte)'A' && _buf[2] == (byte)'L' && _buf[3] == (byte)'B')
                            _expectedSize = FrameSize;            // 405
                        else if (_buf[1] == (byte)'P' && _buf[2] == (byte)'R' && _buf[3] == (byte)'G')
                            _expectedSize = ConfigResponseSize;   // 28
                        else
                        {
                            _reading = false; _idx = 0;           // мусор
                            continue;
                        }
                    }

                    if (_expectedSize > 0 && _idx == _expectedSize)
                    {
                        if (_expectedSize == FrameSize)
                        {
                            // ---- старое поведение для 405 байт ----
                            if (_buf[FrameSize - 2] == 0x0D && _buf[FrameSize - 1] == 0x0A)
                            {
                                var message = ConvertData.GetMessage(_buf);
                                if (!string.IsNullOrEmpty(message))
                                {
                                    DataDelegates.EventHandlerStr(message);
                                    DataDelegates.WriteFHandlerStr(message);
                                    DataDelegates.EventHandlerStrParam(message);
                                }
                                else
                                    DataDelegates.EventHandlerStr("CRC error" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            // ---- ответ конфигуратора ----
                            var copy = new byte[ConfigResponseSize];
                            Array.Copy(_buf, copy, ConfigResponseSize);
                            ConfigPacketReceived?.Invoke(copy);
                        }

                        _reading = false;
                        _idx = 0;
                        _expectedSize = 0;

                        // ⏱ ГЛАВНОЕ: сейчас — «зазор» между кадрами.
                        // Если есть что отправить — отправляем прямо сейчас.
                        TrySendPending();
                    }
                }
            }
            catch (Exception ex)
            {
                _reading = false;
                _idx = 0;
                _expectedSize = 0;
                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
            }
        }

        public void SendBetweenFrames(byte[] data)
        {
            lock (_sendLock) { _pendingSend = data; }

            // Если кадр сейчас не читается — можно отправлять сразу.
            if (!_reading) TrySendPending();
        }

        private void TrySendPending()
        {
            byte[] data;
            lock (_sendLock)
            {
                data = _pendingSend;
                _pendingSend = null;
            }
            if (data == null) return;

            try
            {
                // Небольшая пауза, чтобы устройство точно закончило передачу
                // (например, если метео-канал и конфиг-канал разделены на стороне ПИ)
                // System.Threading.Thread.Sleep(20);

                Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                DataDelegates.EventHandlerStr("Config send error: " + ex.Message + Environment.NewLine);
            }
        }

        public void DataSend(byte[] msg)
        {
            try
            {
                Write(msg, 0, msg.Length);
                DataDelegates.EventHandlerStr(ConvertData.GetMessage(msg));
            }
            catch (Exception ex) { DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine); }
        }

        public bool SendBytes(byte[] data)
        {
            try
            {
                if (!IsOpen) return false;
                Write(data, 0, data.Length);
                return true;
            }
            catch (Exception ex)
            {
                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
                return false;
            }
        }
    }
}
