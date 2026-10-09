using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для CustomWindDataSmall.xaml
    /// </summary>
    public partial class CustomWindDataSmall : UserControl
    {
        List<TextBlock> textBlocks;

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(CustomWindDataSmall), new PropertyMetadata("Датчик"));

        public string Text
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static new readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(CustomWindDataSmall), new PropertyMetadata(Brushes.Orange));

        public new Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public CustomWindDataSmall()
        {
            InitializeComponent();
            //textBlocks = new List<TextBlock>() { DataMinSpeed, DataMidSpeed, DataMaxSpeed, DataMinDir, DataMidDir, DataMaxDir};
            //textBlocks = new List<TextBlock>() { DataSpeed, DataDir };
        }

        public void SetDataToFields(List<string> data)
        {
            //lbl.Text = data[0];
            DataSpeed.Text = data[0];
            DataSpeedLabel.Text = "м/c";
            DataDir.Text = data[1] + "°";
            //for (int i = 0; i < textBlocks.Count; i++)
            //    textBlocks[i].Text = data[i+1];
        }

        public void ClearFields()
        {
            DataSpeed.Text = "Н.Д.";
            DataSpeedLabel.Text = "";
            DataDir.Text =  "Н.Д.";
        }
    }
}
