using Capriz_WPF.Data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;

namespace Capriz_WPF.Common
{
    public class SerialClass : SerialPort
    {
        private int stepIndex;
        private bool startRead;
        private const int dataSize = 357;                 // старый кадр
        private const int ConfigResponseSize = 28;        // PI $PRG
        private byte[] bufer = new byte[dataSize];
        private int _expectedSize = 0;                    // сколько ждать в текущем кадре

        // === НОВОЕ: очередь отправки PI-команд ===
        private readonly object _sendLock = new object();
        private byte[] _pendingSend;

        // === НОВОЕ: событие для PI-конфигуратора ===
        public event Action<byte[]> ConfigPacketReceived;

        public SerialClass(string portName = "COM1", int baudRate = 9600, int dataBits = 8,
            StopBits stopBits = StopBits.One, Parity parity = Parity.None, Handshake handshake = Handshake.None)
        {
            try
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
            catch (Exception err)
            {
                DataDelegates.EventHandlerStr(err.Message + Environment.NewLine);
            }
        }

        public string[] GetSystemPorts()
        {
            return SerialPort.GetPortNames()
                .OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out int num) ? num : 0)
                .ToArray();
        }

        public byte OpenPort(string portName = "COM1", int baudRate = 9600, int dataBits = 8,
            StopBits stopBits = StopBits.One, Parity parity = Parity.None, Handshake handshake = Handshake.None)
        {
            if (IsOpen) Close();
            try
            {
                PortName = portName;
                BaudRate = baudRate;
                DataBits = dataBits;
                Parity = parity;
                StopBits = stopBits;
                Handshake = handshake;
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
            catch (Exception ex)
            {
                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
            }
        }

        // ====================== ПРИЁМ ======================

        void DataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;
            try
            {
                int toRead = port.BytesToRead;
                for (int i = 0; i < toRead; i++)
                {
                    byte b = (byte)port.ReadByte();

                    // Ищем начало кадра — '$'
                    if (!startRead)
                    {
                        if (b == (byte)'$')
                        {
                            stepIndex = 0;
                            bufer[stepIndex++] = b;
                            startRead = true;
                            _expectedSize = 0;
                        }
                        continue;
                    }

                    // Защита от переполнения
                    if (stepIndex >= dataSize)
                    {
                        startRead = false;
                        stepIndex = 0;
                        continue;
                    }

                    bufer[stepIndex++] = b;

                    // Определяем тип кадра по 4-му байту
                    if (stepIndex == 4 && _expectedSize == 0)
                    {
                        if (bufer[1] == (byte)'P' && bufer[2] == (byte)'R' && bufer[3] == (byte)'G')
                            _expectedSize = ConfigResponseSize;   // $PRG → 28
                        else
                            _expectedSize = dataSize;             // старый кадр → 357
                    }

                    // Кадр собран?
                    bool frameComplete = false;
                    if (_expectedSize > 0 && stepIndex == _expectedSize)
                        frameComplete = true;
                    else if (b == 0x0A && _expectedSize == dataSize)   // старый фолбэк: до \n
                        frameComplete = true;

                    if (frameComplete)
                    {
                        if (_expectedSize == ConfigResponseSize)
                        {
                            // ---- PI ответ $PRG ----
                            var copy = new byte[ConfigResponseSize];
                            Array.Copy(bufer, copy, ConfigResponseSize);
                            ConfigPacketReceived?.Invoke(copy);
                        }
                        else
                        {
                            // ---- Старый кадр ----
                            var msg = ConvertData.GetMessage(bufer);
                            DataDelegates.EventHandlerStr(msg);
                            DataDelegates.WriteFHandlerStr(msg);
                            DataDelegates.EventHandlerStrParam(msg);
                        }

                        startRead = false;
                        stepIndex = 0;
                        _expectedSize = 0;
                        bufer = new byte[dataSize];

                        // ⏱ Зазор между кадрами — отправляем PI-команду, если есть
                        TrySendPending();
                    }
                }
            }
            catch (Exception ex)
            {
                startRead = false;
                stepIndex = 0;
                _expectedSize = 0;
                bufer = new byte[dataSize];

                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
            }
        }

        // ====================== ОТПРАВКА PI-КОМАНД ======================

        /// <summary>
        /// Отложенная отправка. Данные уйдут в зазор между кадрами.
        /// </summary>
        public void SendBetweenFrames(byte[] data)
        {
            lock (_sendLock) { _pendingSend = data; }
            if (!startRead) TrySendPending();
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
                Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                DataDelegates.EventHandlerStr("Config send error: " + ex.Message + Environment.NewLine);
            }
        }

        // ====================== СТАРЫЕ МЕТОДЫ ОТПРАВКИ ======================

        public void DataSend(byte[] msg)
        {
            try
            {
                Write(msg, 0, msg.Length);
                DataDelegates.EventHandlerStr(ConvertData.GetMessage(msg));
            }
            catch (Exception ex)
            {
                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
            }
        }

        public void DataSendText(string msg)
        {
            try
            {
                Write(msg);
                DataDelegates.EventHandlerStr(msg);
            }
            catch (Exception ex)
            {
                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
            }
        }

        // ====================== CRC СТАРОГО ПРОТОКОЛА ======================

        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            int ch = 0;
            int control_sum = 0;
            string control_sum_str = "";

            foreach (byte item in message)
            {
                if (item == 42) break;
                ++ch;
            }

            for (byte i = 1; i < ch; ++i)
                control_sum ^= message[i];

            control_sum_str = control_sum.ToString("X2");
            CRC[1] = Convert.ToByte(control_sum_str[0]);
            CRC[0] = Convert.ToByte(control_sum_str[1]);
        }

        private bool CheckResponse(byte[] response)
        {
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            int ch = 0;
            foreach (byte item in response)
            {
                ++ch;
                if (item == 42) break;
            }

            if (CRC[0] == response[ch + 1] && CRC[1] == response[ch])
                return true;
            else
                return false;
        }
    }
}
//using Capriz_WPF.Data;
//using System;
//using System.Collections.Generic;
//using System.IO.Ports;
//using System.Linq;
//using System.Text;
//using System.Windows;

//namespace Capriz_WPF.Common
//{
//    public class SerialClass : SerialPort
//    {
//        private int stepIndex;
//        private bool startRead;
//        private const int dataSize = 357;
//        private byte[] bufer = new byte[dataSize];

//        public SerialClass(string portName = "COM1", int baudRate = 9600, int dataBits = 8,
//            StopBits stopBits = StopBits.One, Parity parity = Parity.None, Handshake handshake = Handshake.None)
//        {
//            try
//            {
//                PortName = portName;
//                BaudRate = baudRate;
//                DataBits = dataBits;
//                StopBits = stopBits;
//                Parity = parity;

//                Handshake = handshake;
//                ReadTimeout = 500;
//                WriteTimeout = 500;

//                DataReceived += DataReceive;
//            }
//            catch (Exception err)
//            {
//                DataDelegates.EventHandlerStr(err.Message + Environment.NewLine);
//            }
//        }

//        public string[] GetSystemPorts()
//        {
//            return SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out int num) ? num : 0).ToArray();//отсортировать
//        }


//        public byte OpenPort(string portName = "COM1", int baudRate = 9600, int dataBits = 8,
//            StopBits stopBits = StopBits.One, Parity parity = Parity.None, Handshake handshake = Handshake.None)
//        {
//            if (IsOpen)
//            {
//                Close();
//            }
//            try
//            {
//                PortName = portName;
//                BaudRate = baudRate;
//                DataBits = dataBits;
//                Parity = parity;
//                StopBits = stopBits;
//                Handshake = handshake;
//                Open();
//                DiscardOutBuffer();
//                DiscardInBuffer();
//                DataDelegates.EventHandlerStr(portName + " is open." + Environment.NewLine);
//                return 1;
//            }
//            catch (Exception err)
//            {
//                DataDelegates.EventHandlerStr(err.Message + Environment.NewLine);
//                return 0;
//            }

//        }

//        //закрытие COM-порта
//        public void ClosePort()
//        {
//            try
//            {
//                if (IsOpen)
//                {
//                    Close();
//                    DataDelegates.EventHandlerStr(PortName + " is close." + Environment.NewLine);
//                }
//            }
//            catch (Exception ex)
//            {
//                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
//                return;
//            }
//        }

//        void DataReceive(object sender, SerialDataReceivedEventArgs e)
//        {
//            var port = (SerialPort)sender;
//            byte prev_bt = 36;
//            try
//            {
//                var buferSize = port.BytesToRead;
//                for (int i = 0; i < buferSize; ++i)
//                {
//                    var bt = (byte)port.ReadByte();
//                    if (0x24 == bt && prev_bt == 36)//$
//                    {
//                        stepIndex = 0;
//                        startRead = true;
//                    }
//                    if (startRead)
//                    {
//                        bufer[stepIndex] = bt;
//                        prev_bt = bt;
//                        ++stepIndex;
//                    }
//                    if (stepIndex > 1 && bufer[stepIndex - 1] == 0xA && startRead)
//                    {
//                        DataDelegates.EventHandlerStr(ConvertData.GetMessage(bufer));
//                        DataDelegates.WriteFHandlerStr(ConvertData.GetMessage(bufer));
//                        DataDelegates.EventHandlerStrParam(ConvertData.GetMessage(bufer));


//                        startRead = false;
//                        bufer = new byte[dataSize];
//                        prev_bt = 255;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                startRead = false;
//                //индекс вышел за пределы массива
//                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine
//                + "index=" + stepIndex + Environment.NewLine);
//                bufer = new byte[dataSize];
//                prev_bt = 36;
//            }
//        }

//        public void DataSend(byte[] msg)
//        {
//            try
//            {
//                Write(msg, 0, msg.Length);
//                DataDelegates.EventHandlerStr(ConvertData.GetMessage(msg));
//            }
//            catch (Exception ex)
//            {
//                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
//            }
//        }

//        public void DataSendText(string msg)
//        {
//            try
//            {
//                Write(msg);
//                DataDelegates.EventHandlerStr(msg);
//            }
//            catch (Exception ex)
//            {
//                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine);
//            }
//        }


//        private void GetCRC(byte[] message, ref byte[] CRC)
//        {
//            int ch = 0;
//            int control_sum = 0;
//            string control_sum_str = "";
//            foreach (byte item in message)
//            {
//                if (item == 42) break;
//                ++ch;
//            }

//            for (byte i = 1; i < ch; ++i)
//            {
//                control_sum ^= message[i];
//            }

//            control_sum_str = control_sum.ToString("X2");
//            CRC[1] = Convert.ToByte(control_sum_str[0]);
//            CRC[0] = Convert.ToByte(control_sum_str[1]);
//        }

//        //проверка КС принятого сообщения 
//        private bool CheckResponse(byte[] response)
//        {
//            //Perform a basic CRC check:
//            byte[] CRC = new byte[2];
//            GetCRC(response, ref CRC);
//            int ch = 0;
//            foreach (byte item in response)
//            {
//                ++ch;
//                if (item == 42) break;
//            }

//            if (CRC[0] == response[ch + 1] && CRC[1] == response[ch])
//                return true;
//            else
//                return false;
//        }
//    }
//}
