using Capriz_WPF.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Capriz_WPF.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomBottomDataPanel.xaml
    /// </summary>
    public partial class CustomBottomDataPanel : System.Windows.Controls.UserControl
    {
        MainWindow mw = System.Windows.Application.Current.MainWindow as MainWindow;
        //string _toolTipText = "";


        //public string ToolTipText { get => _toolTipText; set => _toolTipText = value; }

        List<string> mydata;
        //bool isMouseEnter = false;
        //DispatcherTimer timer;

        public CustomBottomDataPanel()
        {
            InitializeComponent();

            DataSost.Fill = new SolidColorBrush(Color.FromRgb(192, 192, 192));
            DataSost.Stroke = new SolidColorBrush(Color.FromRgb(162, 162, 162));
        }

        public void SetDataToSost(List<string> data)
        {
            mydata = data;

            if (data == null || data.Count == 0) return;

            // Цвет по статусу
            var color = data[0] == "1"
                ? new SolidColorBrush(Color.FromRgb(88, 215, 88))
                : data[0] == "0"
                    ? new SolidColorBrush(Color.FromRgb(255, 64, 64))
                    : new SolidColorBrush(Color.FromRgb(192, 192, 192));

            DataSost.Fill = color;
            DataSost.Stroke = color;

            // Ошибки — всё кроме статуса
            var errors = data.Skip(1).ToList();

            // Обновляем открытую панель
            if (errorPanel.IsOpen)
            {
                if (errors.Count > 0)
                    errorPanel.RefreshErrors(errors);
                else
                    errorPanel.HideErrors();
            }
        }

        private void DataSost_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mydata == null || mydata.Count < 2)
                return;

            var errors = mydata.Skip(1).ToList();
            if (errors.Count == 0)
                return;

            if (errorPanel.IsOpen)
                errorPanel.HideErrors();
            else
                errorPanel.ShowErrors(errors);
        }
    }
}
