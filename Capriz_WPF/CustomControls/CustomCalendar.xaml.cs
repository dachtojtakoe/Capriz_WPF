using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

using static Capriz_WPF.MainWindow;

namespace Capriz_WPF.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomCalendar.xaml
    /// </summary>
    public partial class CustomCalendar : UserControl
    {
        CultureInfo culture = new CultureInfo("ru-Ru");

        //private DateTime MinMaxDt;

        int clickedTimeBtn = 0;

        private DateTime initialDisplayDate;
        private DateTime? initialSelectedDate;
        MainWindow mw = Application.Current.MainWindow as MainWindow;
        int _calledby = 0;
        public enum calledby : int
        {
            DateFrom = 1,
            DateTo = 2,
            DateFromlbl = 3,
            DateTolbl = 4,
            DateTime = 5
        };

        public int CalledBy { get => _calledby; set => _calledby = value; }


        public CustomCalendar()
        {
            InitializeComponent();
            Loaded += CustomCalendar_Loaded;
            IsVisibleChanged += CustomCalendar_IsVisibleChanged;
        }

        private void CustomCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            SetTime();
            initialDisplayDate = calendar1.DisplayDate;
            initialSelectedDate = calendar1.SelectedDate;
        }

        public void SetData(DateTime dateTime)
        {
            //if ((_calledby == (int)calledby.DateFrom) || (_calledby == (int)calledby.DateTo))
            //    MinMaxDt = dateTime;
            calendar1.SelectedDate = dateTime.Date;
            calendar1.DisplayDate = dateTime.Date;
            btnHours.Content = dateTime.Hour;
            btnMinutes.Content = dateTime.Minute;
            btnSeconds.Content = dateTime.Second;
        }

        private void CustomCalendar_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible == false)
            {
                // Восстанавливаем состояние при скрытии календаря
                calendar1.DisplayDate = initialDisplayDate;
                calendar1.SelectedDate = initialSelectedDate;
                SetButtonFirstColor(btnHours);
                SetButtonFirstColor(btnMinutes);
                SetButtonFirstColor(btnSeconds);
                SetTime();
                clickedTimeBtn = 0;
            }
        }

        private void SetTime()
        {
            btnHours.Content = DateTime.Now.Hour.ToString("00");
            btnMinutes.Content = DateTime.Now.Minute.ToString("00");
            btnSeconds.Content = DateTime.Now.Second.ToString("00");
        }

        private void BtnTimeAdd_Click(object sender, RoutedEventArgs e)
        {
            var hour = Convert.ToInt32(btnHours.Content);
            var min = Convert.ToInt32(btnMinutes.Content);
            var sec = Convert.ToInt32(btnSeconds.Content);

            if (clickedTimeBtn == 1)
            {
                hour = (hour + 1) % 24;
                btnHours.Content = hour.ToString("00");
            }
            else if (clickedTimeBtn == 2)
            {
                min = (min + 1) % 60;
                btnMinutes.Content = min.ToString("00");
            }
            else if (clickedTimeBtn == 3)
            {
                sec = (sec + 1) % 60;
                btnSeconds.Content = sec.ToString("00");
            }
        }

        private void BtnTimeOdd_Click(object sender, RoutedEventArgs e)
        {
            var hour = Convert.ToInt32(btnHours.Content);
            var min = Convert.ToInt32(btnMinutes.Content);
            var sec = Convert.ToInt32(btnSeconds.Content);

            if (clickedTimeBtn == 1)
            {
                hour = (hour + 23) % 24;
                btnHours.Content = hour.ToString("00");
            }
            else if (clickedTimeBtn == 2)
            {
                min = (min + 59) % 60;
                btnMinutes.Content = min.ToString("00");
            }
            else if (clickedTimeBtn == 3)
            {
                sec = (sec + 59) % 60;
                btnSeconds.Content = sec.ToString("00");
            }
        }

        private void BtnTimes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Focus();
        }

        private void btnHours_Click(object sender, RoutedEventArgs e)
        {
            clickedTimeBtn = 1;
            ChangeButtonBorderColor(btnHours, Colors.White);
            SetButtonFirstColor(btnMinutes);
            SetButtonFirstColor(btnSeconds);
        }

        private void btnMinutes_Click(object sender, RoutedEventArgs e)
        {
            clickedTimeBtn = 2;
            ChangeButtonBorderColor(btnMinutes, Colors.White);
            SetButtonFirstColor(btnHours);
            SetButtonFirstColor(btnSeconds);
        }

        private void btnSeconds_Click(object sender, RoutedEventArgs e)
        {
            clickedTimeBtn = 3;
            ChangeButtonBorderColor(btnSeconds, Colors.White);
            SetButtonFirstColor(btnHours);
            SetButtonFirstColor(btnMinutes);
        }

        private void ChangeButtonBorderColor(Button button, Color newColor)
        {
            if (button.Template.FindName("Border", button) is Border border)
            {
                border.BorderBrush = new SolidColorBrush(newColor);
            }
        }

        private void SetButtonFirstColor(Button button)
        {
            GradientStopCollection gradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromRgb(30, 70, 91), 0.0),
                new GradientStop(Color.FromRgb(61,96,135), 1.0)
            };

            // Создаем линейный градиент
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(gradientStops)
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1)
            };

            if (button.Template.FindName("Border", button) is Border border)
            {
                border.BorderBrush = linearGradientBrush;
            }
        }

        //private void CalendarDayButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    MessageBox.Show("");

        //    if (sender is CalendarDayButton dayButton)
        //    {
        //        DateTime selectedDate = (DateTime)dayButton.DataContext;
        //        MessageBox.Show($"Выбранная дата: {selectedDate.ToShortDateString()}");
        //    }
        //}

        private void btnOk_Click(object sender, EventArgs e)
        {
            DateTime d;
            DateTime t;
            string str = btnHours.Content + ":" + btnMinutes.Content + ":" + btnSeconds.Content;
            if (calendar1.SelectedDate != null)
                d = Convert.ToDateTime(calendar1.SelectedDate);
            else
                d = Convert.ToDateTime(calendar1.DisplayDate);
            t = Convert.ToDateTime(str);
            d = d.Date + t.TimeOfDay;

            this.Visibility = Visibility.Hidden;

            if (_calledby == (int)calledby.DateFrom)
                mw.btnDateFrom.BtnText = d > Convert.ToDateTime(mw.btnDateTo.BtnText) ? mw.btnDateTo.BtnText : d < mw.DtMin ? mw.DtMin.ToString() : d.ToString();
            else if (_calledby == (int)calledby.DateTo)
                mw.btnDateTo.BtnText = d < Convert.ToDateTime(mw.btnDateFrom.BtnText) ? mw.btnDateFrom.BtnText : d > mw.DtMax ? mw.DtMax.ToString() : d.ToString();
            else if (_calledby == (int)calledby.DateTime)
            {
                try
                {
                    SystemTimeChanger.SetSystemTime(d);
                    //System.Windows.Forms.MessageBox.Show("System time has been changed: " + d);
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show("Failed to change system time: " + ex.Message + " " + d );
                }
            }
            else
            {
                if (_calledby == (int)calledby.DateFromlbl)
                    mw.btnlblDateFrom.BtnText = d > Convert.ToDateTime(mw.btnlblDateTo.BtnText) ? mw.btnlblDateTo.BtnText : d < mw.DtMin ? mw.DtMin.ToString() : d.ToString();
                else if (_calledby == (int)calledby.DateTolbl)
                    mw.btnlblDateTo.BtnText = d < Convert.ToDateTime(mw.btnlblDateFrom.BtnText) ? mw.btnlblDateFrom.BtnText : d > mw.DtMax ? mw.DtMax.ToString() : d.ToString();

                if (mw.ViewNow == (int)_typeWindow.customGrid1)
                    mw.ShowGrid(mw.btnlblDateFrom.BtnText, mw.btnlblDateTo.BtnText);
                if (mw.ViewNow == (int)_typeWindow.customChart1)
                    mw.ShowChart(mw.btnlblDateFrom.BtnText, mw.btnlblDateTo.BtnText);
                mw.isPanelOpened = true;
            }
        }

        private void calendar1_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
    }

    public class SystemTimeChanger
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        public static void SetSystemTime(DateTime newTime)
        {
            DateTime utcTime = newTime.ToUniversalTime();
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            SYSTEMTIME st = new SYSTEMTIME
            {
                wYear = (short)utcTime.Year,
                wMonth = (short)utcTime.Month,
                wDay = (short)utcTime.Day,
                wHour = (short)utcTime.Hour,
                wMinute = (short)utcTime.Minute,
                wSecond = (short)utcTime.Second,
                wMilliseconds = (short)utcTime.Millisecond
            };

            if (!SetSystemTime(ref st))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
