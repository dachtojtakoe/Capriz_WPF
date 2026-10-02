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
using System.Windows.Navigation;
using System.Windows.Shapes;
 
namespace Capriz_WPF.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для PortSettingsRow.xaml
    /// </summary>
    public partial class PortSettingsRow : UserControl
    {
        public static readonly string[] BaudRates = { "4800", "9600", "19200", "38400", "57600", "115200" };
        private readonly CheckBox[] _recvCheckBoxes;
        private bool _applying;

        public PortSettingsRow()
        {
            InitializeComponent();
            baudCombo.ItemsSource = BaudRates;
            baudCombo.SelectedIndex = 0;
            _recvCheckBoxes = new[] { recvVTG, recvVHW, recvRMC, recvVBW, recvHDT };
        }

        public static readonly DependencyProperty PortNameProperty = DependencyProperty.Register(nameof(PortName), typeof(string), typeof(PortSettingsRow), new PropertyMetadata(""));

        public string PortName
        {
            get => (string)GetValue(PortNameProperty);
            set => SetValue(PortNameProperty, value);
        }

        public static readonly DependencyProperty BaudIndexProperty = DependencyProperty.Register(nameof(BaudIndex), typeof(int), typeof(PortSettingsRow), new PropertyMetadata(0));
        public int BaudIndex
        {
            get => (int)GetValue(BaudIndexProperty);
            set => SetValue(BaudIndexProperty, value);
        }

        public static readonly DependencyProperty RxIndexProperty =
            DependencyProperty.Register(nameof(RxIndex), typeof(int),
                typeof(PortSettingsRow),
                new PropertyMetadata(0, OnRxIndexChanged));

        public int RxIndex
        {
            get => (int)GetValue(RxIndexProperty);
            set => SetValue(RxIndexProperty, value);
        }

        public static readonly DependencyProperty TxMaskProperty = DependencyProperty.Register(nameof(TxMask), typeof(int), typeof(PortSettingsRow), new PropertyMetadata(0, OnTxMaskChanged));

        public int TxMask
        {
            get => (int)GetValue(TxMaskProperty);
            set => SetValue(TxMaskProperty, value);
        }

        public static readonly DependencyProperty RxVTGProperty = RegBool(nameof(RxVTG), OnRxFlagChanged); 
        public static readonly DependencyProperty RxVHWProperty = RegBool(nameof(RxVHW), OnRxFlagChanged);
        public static readonly DependencyProperty RxRMCProperty = RegBool(nameof(RxRMC), OnRxFlagChanged);
        public static readonly DependencyProperty RxVBWProperty = RegBool(nameof(RxVBW), OnRxFlagChanged);
        public static readonly DependencyProperty RxHDTProperty = RegBool(nameof(RxHDT), OnRxFlagChanged);

        public static readonly DependencyProperty TxALBProperty = RegBool(nameof(TxALB), OnTxFlagChanged);
        public static readonly DependencyProperty TxMWVProperty = RegBool(nameof(TxMWV), OnTxFlagChanged);
        public static readonly DependencyProperty TxXDRProperty = RegBool(nameof(TxXDR), OnTxFlagChanged);
        public static readonly DependencyProperty TxMWDProperty = RegBool(nameof(TxMWD), OnTxFlagChanged);

        public bool RxVTG { get => (bool)GetValue(RxVTGProperty); set => SetValue(RxVTGProperty, value); }
        public bool RxVHW { get => (bool)GetValue(RxVHWProperty); set => SetValue(RxVHWProperty, value); }
        public bool RxRMC { get => (bool)GetValue(RxRMCProperty); set => SetValue(RxRMCProperty, value); }
        public bool RxVBW { get => (bool)GetValue(RxVBWProperty); set => SetValue(RxVBWProperty, value); }
        public bool RxHDT { get => (bool)GetValue(RxHDTProperty); set => SetValue(RxHDTProperty, value); }

        public bool TxALB { get => (bool)GetValue(TxALBProperty); set => SetValue(TxALBProperty, value); }
        public bool TxMWV { get => (bool)GetValue(TxMWVProperty); set => SetValue(TxMWVProperty, value); }
        public bool TxXDR { get => (bool)GetValue(TxXDRProperty); set => SetValue(TxXDRProperty, value); }
        public bool TxMWD { get => (bool)GetValue(TxMWDProperty); set => SetValue(TxMWDProperty, value); }

        private static DependencyProperty RegBool(string name, PropertyChangedCallback cb)
        {
            return DependencyProperty.Register(name, typeof(bool),
                typeof(PortSettingsRow), new PropertyMetadata(false, cb));
        }

        private static void OnRxIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = (PortSettingsRow)d;
            int idx = (int)e.NewValue;
            row._applying = true;
            row.RxVTG = idx == 1;
            row.RxVHW = idx == 2;
            row.RxRMC = idx == 3;
            row.RxVBW = idx == 4;
            row.RxHDT = idx == 5;
            row._applying = false;
        }

        private static void OnRxFlagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = (PortSettingsRow)d;
            if (row._applying) return;
            // Взаимоисключение: оставляем только один активный
            row.RxIndex = row.RxVTG ? 1
                        : row.RxVHW ? 2
                        : row.RxRMC ? 3
                        : row.RxVBW ? 4
                        : row.RxHDT ? 5
                        : 0;
        }

        private static void OnTxMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = (PortSettingsRow)d;
            int m = (int)e.NewValue;
            row._applying = true;
            row.TxALB = (m & 1) != 0;
            row.TxMWV = (m & 2) != 0;
            row.TxXDR = (m & 4) != 0;
            row.TxMWD = (m & 8) != 0;    // ← 8, а не 16
            row._applying = false;
        }

        private static void OnTxFlagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = (PortSettingsRow)d;
            if (row._applying) return;
            row._applying = true;
            row.TxMask = (row.TxALB ? 1 : 0)
                       | (row.TxMWV ? 2 : 0)
                       | (row.TxXDR ? 4 : 0)
                       | (row.TxMWD ? 8 : 0);   // ← было 16, должно быть 8
            row._applying = false;
        }

        private void RecvCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (_applying) return;
            var cur = sender as CheckBox;
            foreach (var cb in _recvCheckBoxes)
                if (cb != cur) cb.IsChecked = false;
        }
    }
}
