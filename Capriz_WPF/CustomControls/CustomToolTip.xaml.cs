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
    /// Логика взаимодействия для CustomToolTip.xaml
    /// </summary>
    public partial class CustomToolTip : UserControl
    {
        public static readonly DependencyProperty ToolTipWidthProperty =
            DependencyProperty.Register("ToolTipWidth", typeof(double), typeof(CustomToolTip), new PropertyMetadata(100.0));

        public static readonly DependencyProperty ToolTipHeightProperty =
            DependencyProperty.Register("ToolTipHeight", typeof(double), typeof(CustomToolTip), new PropertyMetadata(null));

        public static readonly DependencyProperty ToolTipTextProperty =
            DependencyProperty.Register("ToolTipText", typeof(string), typeof(CustomToolTip), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ToolTipTextForegroundProperty =
            DependencyProperty.Register("ToolTipTextForeground", typeof(Brush), typeof(CustomToolTip), new PropertyMetadata(Brushes.AliceBlue));

        public static readonly DependencyProperty ToolTipBorderColorProperty=
            DependencyProperty.Register("ToolTipBorderColor", typeof(Brush), typeof(CustomToolTip), new PropertyMetadata(Brushes.AliceBlue));


        public double ToolTipWidth
        {
            get { return (double)GetValue(ToolTipWidthProperty); }
            set { SetValue(ToolTipWidthProperty, value); }
        }

        public double ToolTipHeight
        {
            get { return (double)GetValue(ToolTipHeightProperty); }
            set { SetValue(ToolTipHeightProperty, value); }
        }

        public string ToolTipText
        {
            get { return (string)GetValue(ToolTipTextProperty); }
            set { SetValue(ToolTipTextProperty, value); }
        }

        public Brush ToolTipTextForeground
        {
            get { return (Brush)GetValue(ToolTipTextForegroundProperty); }
            set { SetValue(ToolTipTextForegroundProperty, value); }
        }


        public Brush ToolTipBorderColor
        {
            get { return (Brush)GetValue(ToolTipBorderColorProperty); }
            set { SetValue(ToolTipBorderColorProperty, value); }
        }

        public CustomToolTip()
        {
            InitializeComponent();
            this.DataContext = this;

        }
    }
}
