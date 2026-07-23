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

        readonly List<TextBlock> _textBlocksShip;
        readonly List<TextBlock> _textBlocksDateTime;

        string _toolTipText = "";
        public string ToolTipText { get => _toolTipText; set => _toolTipText = value; }

        List<string> mydata; 
        bool isMouseEnter = false;
        DispatcherTimer timer;

        public CustomBottomDataPanel()
        {
            InitializeComponent();

            DataSost.Fill = new SolidColorBrush(Color.FromRgb(192, 192, 192));
            DataSost.Stroke = new SolidColorBrush(Color.FromRgb(162, 162, 162));

            _textBlocksShip = new List<TextBlock>() { DataCurs, DataSpeed };
            _textBlocksDateTime = new List<TextBlock>() { DataDate, DataTime };

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += Timer_Tick;
        }

        public void SetDataToFields(List<string> data)
        {
            for (int i = 0; i < _textBlocksShip.Count; i++)
                _textBlocksShip[i].Text = data[i];
        }

        public void ClearShipFields()
        {
            for (int i = 0; i < _textBlocksShip.Count; i++)
                _textBlocksShip[i].Text = "Н.Д.";
        }

        public void SetDateTime(List<string> data)
        {
            for (int i = 0; i < _textBlocksDateTime.Count; i++)
                _textBlocksDateTime[i].Text = data[i];
        }
        public void SetDataToSost(List<string> data)
        {
            mydata = data;
            if (data != null)
            {
                if (!isMouseEnter)
                {
                    DataSost.Fill = data[0] == "1" ? new SolidColorBrush(Color.FromRgb(88, 215, 88)) : data[0] == "0" ? new SolidColorBrush(Color.FromRgb(255, 64, 64)) : new SolidColorBrush(Color.FromRgb(192, 192, 192));
                    DataSost.Stroke = data[0] == "1" ? new SolidColorBrush(Color.FromRgb(88, 215, 88)) : data[0] == "0" ? new SolidColorBrush(Color.FromRgb(255, 64, 64)) : new SolidColorBrush(Color.FromRgb(192, 192, 192));
                }
                else
                {
                    DataSost.Fill = data[0] == "1" ? new SolidColorBrush(Color.FromRgb(88, 215, 88)) : data[0] == "0" ? new SolidColorBrush(Color.FromRgb(255, 64, 64)) : new SolidColorBrush(Color.FromRgb(192, 192, 192));
                    DataSost.Stroke = data[0] == "1" ? new SolidColorBrush(Color.FromRgb(88, 215, 88)) : data[0] == "0" ? new SolidColorBrush(Color.FromRgb(255, 64, 64)) : new SolidColorBrush(Color.FromRgb(192, 192, 192));
                }

                CustomToolTip1.Visibility = Visibility.Hidden;
                if (data[1] != "")
                {
                    CustomToolTip1.Visibility = Visibility.Visible;
                    Rect initialPlacementRectangle = new Rect(-95, -33, 0, 0);
                    CustomToolTipPopup.Height = 30;
                    CustomToolTipPopup.Width = 125;
                    CustomToolTip1.ToolTipHeight = 30;
                    CustomToolTip1.ToolTipWidth = 125;


                    CustomToolTip1.ToolTipText = data[1];
                    CustomToolTip1.Dispatcher.BeginInvoke(new Action(() => { }), System.Windows.Threading.DispatcherPriority.Render);
                    int count = CountCarriageReturnNewLines(data[1]); //Подсчет количество строк (символов переноса строки)
                    if (count > 0)
                    {
                        CustomToolTipPopup.Height += 16 * (count - 1);
                        CustomToolTipPopup.Width += 175;
                        initialPlacementRectangle.Y -= 16 * (count - 1);
                        initialPlacementRectangle.X -= 175;
                        CustomToolTip1.ToolTipHeight += 16 * (count - 1);
                        CustomToolTip1.ToolTipWidth += 175;
                    }

                    CustomToolTipPopup.PlacementRectangle = initialPlacementRectangle;
                }
            }
        }

        private void DataSost_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseEnter = true;
            SetDataToSost(mydata);
            CustomToolTipPopup.IsOpen = true;
            timer.Start();
        }

        private void DataSost_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseEnter = false;
            SetDataToSost(mydata);
            CustomToolTipPopup.IsOpen = false;
            timer.Stop();
        }

        private int CountCarriageReturnNewLines(string text)
        {
            int count = 0;
            int index = 0;

            while ((index = text.IndexOf("\r\n", index)) != -1)
            {
                count++;
                index += 2; 
            }
            return count;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Останавливаем таймер
            timer.Stop();

            // Закрываем подсказку, если курсор мыши больше не на элементе
            if (isMouseEnter)
            {
                CustomToolTipPopup.IsOpen = false;
            }
        }

        private void DataSost_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseEnter = true;
            SetDataToSost(mydata);
            CustomToolTipPopup.IsOpen = true;
            timer.Start();
        }

        private void DataTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mw.customCalendar1.SetData(Convert.ToDateTime(DataDate.Text + " " + DataTime.Text));
            mw.customCalendar1.CalledBy = 5;
            mw.customCalendar1.Visibility = Visibility.Visible;
        }
    }
}
