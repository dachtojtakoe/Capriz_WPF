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
        private const int dataSize = 357;
        private byte[] bufer = new byte[dataSize];

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
            return SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out int num) ? num : 0).ToArray();//отсортировать
        }


        public byte OpenPort(string portName = "COM1", int baudRate = 9600, int dataBits = 8,
            StopBits stopBits = StopBits.One, Parity parity = Parity.None, Handshake handshake = Handshake.None)
        {
            if (IsOpen)
            {
                Close();
            }
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

        //закрытие COM-порта
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
                return;
            }
        }

        void DataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;
            byte prev_bt = 36;
            try
            {
                var buferSize = port.BytesToRead;
                for (int i = 0; i < buferSize; ++i)
                {
                    var bt = (byte)port.ReadByte();
                    if (0x24 == bt && prev_bt == 36)//$
                    {
                        stepIndex = 0;
                        startRead = true;
                    }
                    if (startRead)
                    {
                        bufer[stepIndex] = bt;
                        prev_bt = bt;
                        ++stepIndex;
                    }
                    if (stepIndex > 1 && bufer[stepIndex - 1] == 0xA && startRead)
                    {
                        DataDelegates.EventHandlerStr(ConvertData.GetMessage(bufer));
                        DataDelegates.WriteFHandlerStr(ConvertData.GetMessage(bufer));
                        DataDelegates.EventHandlerStrParam(ConvertData.GetMessage(bufer));


                        startRead = false;
                        bufer = new byte[dataSize];
                        prev_bt = 255;
                    }
                }
            }
            catch (Exception ex)
            {
                startRead = false;
                //индекс вышел за пределы массива
                DataDelegates.EventHandlerStr(ex.Message + Environment.NewLine
                + "index=" + stepIndex + Environment.NewLine);
                bufer = new byte[dataSize];
                prev_bt = 36;
            }
        }

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
            {
                control_sum ^= message[i];
            }

            control_sum_str = control_sum.ToString("X2");
            CRC[1] = Convert.ToByte(control_sum_str[0]);
            CRC[0] = Convert.ToByte(control_sum_str[1]);
        }

        //проверка КС принятого сообщения 
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
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
