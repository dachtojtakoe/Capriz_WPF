using Capriz_WPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Capriz_WPF.CustomControls;
using Capriz_WPF.Common;

namespace Capriz_WPF.Views
{
    /// <summary>
    /// Логика взаимодействия для PIConfigurationWindow.xaml
    /// </summary>
    public partial class PIConfigurationWindow : Window
    {
        private readonly SerialClass _serial;
        private PortSettingsRow[] _rows;
        //private PIConfiguratorProtocol _proto;

        public PIConfigurationWindow(SerialClass serial)
        {
            InitializeComponent();
            _serial = serial;

            _rows = new[] { rowX8, rowX4, rowX2, rowX3, rowX5, rowX6, rowX7 };

            _serial.ConfigPacketReceived += OnConfigPacket;
            Loaded += OnLoaded;
            Closed += (_, __) => _serial.ConfigPacketReceived -= OnConfigPacket;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_serial.IsOpen)
                {
                    var get = PIConfiguratorProtocol.BuildGet();
                    _serial.SendBetweenFrames(get);
                }
                else
                {
                    System.Windows.MessageBox.Show("COM-порт не открыт", "Конфигуратор ПИ");
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
        //private bool TryOpenPort()
        //{
        //    //string portName = "COM" + (comPortCombo.SelectedIndex + 1);
        //    string portName = "COM" + 2;
        //    int baud = 9600; // uoCheckBox.IsChecked == true ? 9600 : 115200;

        //    try
        //    {
        //        _proto.Close();
        //        _proto.Open(portName, baud);
        //        return true;
        //    }
        //    catch
        //    {
        //        System.Windows.MessageBox.Show(
        //            "COM порт отсутствует в системе или указан неверно!",
        //            "Конфигуратор ПИ", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //}

        private void WriteConfigurationBtn_Click(object sender, RoutedEventArgs e)
        {
            var set = BuildSetFromUi();
            _serial.SendBetweenFrames(set);   // уйдёт в зазоре между кадрами
        }

        private void ReadConfigurationBtn_Click(object sender, RoutedEventArgs e)
        {
            var get = PIConfiguratorProtocol.BuildGet();   // 7 байт
            _serial.SendBetweenFrames(get);
        }

        private void EscapeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();   // порт не закрываем — он общий!
        }

        private byte[] BuildSetFromUi()
        {
            int windType = windTypeCombo.SelectedIndex;
            int averaging = windAverageCombo.SelectedIndex;

            int[] baud = new int[7];
            int[] rx = new int[7];
            int[] tx = new int[7];

            for (int i = 0; i < _rows.Length; i++)
            {
                baud[i] = _rows[i].BaudIndex;
                rx[i] = _rows[i].RxIndex;     // 0..5 (индекс, а не маска!)
                tx[i] = _rows[i].TxMask;      // 0..15
            }

            return PIConfiguratorProtocol.BuildSet(windType, averaging, baud, rx, tx);
        }

        private void OnConfigPacket(byte[] p)
        {
            if (!PIConfiguratorProtocol.TryParseResponse(p, out var data)) return;

            Dispatcher.Invoke(() =>
            {
                windTypeCombo.SelectedIndex = data.WindTypeIndex;
                windAverageCombo.SelectedIndex = data.AveragingIndex;

                for (int i = 0; i < _rows.Length && i < data.BaudPerPort.Length; i++)
                {
                    _rows[i].BaudIndex = data.BaudPerPort[i];
                    _rows[i].RxIndex = data.RxPerPort[i];   // см. замечание ниже
                    _rows[i].TxMask = data.TxPerPort[i];
                }
            });
        }

        //private void OnPacketReceived(byte[] p)
        //{
        //    if (!PIConfiguratorProtocol.TryParseResponse(p, out var data)) return;

        //    Dispatcher.Invoke(() =>
        //    {
        //        // Глобальные ComboBox'ы
        //        windTypeCombo.SelectedIndex = data.WindTypeIndex;
        //        windAverageCombo.SelectedIndex = data.AveragingIndex;

        //        // 7 строк
        //        for (int i = 0; i < _rows.Length && i < data.BaudPerPort.Length; i++)
        //        {
        //            _rows[i].BaudIndex = data.BaudPerPort[i];
        //            _rows[i].RxIndex = data.RxPerPort[i];
        //            _rows[i].TxMask = data.TxPerPort[i];
        //        }
        //    });
        //}
    }
}
