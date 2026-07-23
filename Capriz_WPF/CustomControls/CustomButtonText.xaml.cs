using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для CustomButtonText.xaml
    /// </summary>
    public partial class CustomButtonText : UserControl
    {

        public static readonly DependencyProperty CommandProperty =
           DependencyProperty.Register("CommandText", typeof(ICommand), typeof(CustomButtonText), new PropertyMetadata(null));

        public ICommand CommandText
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty BtnTextProperty =
            DependencyProperty.Register("BtnText", typeof(string), typeof(CustomButtonText), new PropertyMetadata("Ok"));

        public string BtnText
        {
            get { return (string)GetValue(BtnTextProperty); }
            set { SetValue(BtnTextProperty, value); }
        }

        public static readonly DependencyProperty BtnFontSizeProperty =
            DependencyProperty.Register("BtnFontSize", typeof(double), typeof(CustomButtonText), new PropertyMetadata(21.0));

        public double BtnFontSize
        {
            get { return (double)GetValue(BtnFontSizeProperty); }
            set { SetValue(BtnFontSizeProperty, value); }
        }

        public static readonly DependencyProperty BtnFontWeightProperty =
    DependencyProperty.Register("BtnFontWeight", typeof(FontWeight), typeof(CustomButtonText), new PropertyMetadata(FontWeights.Bold));

        public FontWeight BtnFontWeight
        {
            get { return (FontWeight)GetValue(BtnFontWeightProperty); }
            set { SetValue(BtnFontWeightProperty, value); }
        }

        public CustomButtonText()
        {
            InitializeComponent();
            this.DataContext = this;
            CustomButtonControlText.Click += CustomButtonControlText_Click;
        }

        private void CustomButtonControlText_Click(object sender, RoutedEventArgs e)
        {
            if (CommandText != null && CommandText.CanExecute(null))
            {
                CommandText.Execute(null);
            }
        }
    }
}
