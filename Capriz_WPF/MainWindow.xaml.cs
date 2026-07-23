using Capriz_WPF.Common;
using Capriz_WPF.CustomControls;
using Capriz_WPF.Data;
using System;
using System.Data;
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
using System.Xml.Serialization;
using System.Windows.Forms;
using Capriz_WPF.Properties;
using System.Windows.Threading;
using System.Xml;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using OxyPlot;
using System.Runtime.Caching;
using System.Diagnostics;
using Capriz_WPF.Database;
using System.Windows.Markup;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using PrintDialog = System.Windows.Controls.PrintDialog;

using Point = System.Windows.Point;
using System.Xml.Linq;
using System.Printing;

namespace Capriz_WPF
{
    public partial class MainWindow : Window
    {
        DataTable _dt = new DataTable();
        SerialClass _serial = new SerialClass();
        IniFile _iniFile = new IniFile(DataFile.SettingsPath());
        Configuration _config = new Configuration();

        int _viewNow;
        public int ViewNow { get => _viewNow; set => _viewNow = value; }

        string chartNow = "";
        public string ChartNow { get => chartNow; set => chartNow = value; }

        DateTime dtMin, dtMax;
        public DateTime DtMin { get => dtMin; set => dtMin = value; }
        public DateTime DtMax { get => dtMax; set => dtMax = value; }

        int btnPeriodClicked = 0;

        string _nameCurrentFile = "";

        public enum _typeWindow : int
        {
            customWindPanel1 = 0,
            customGrid1 = 1,
            customChart1 = 2,
            customTerminal = 3
        };

        private DispatcherTimer _timer;
        private CustomToolTip _currentToolTip;

        /// <summary>
        /// Переменная говорящая - открыто ли окно приложения или находится в промежуточном положении (например открыт календарь)
        /// </summary>
        public bool isPanelOpened = true;

        private bool isScrollToEnd = true; //Автоматическое перемещение к последнему сообщению на экране терминального отображения

        public MainWindow()
        {
            //if (Environment.OSVersion.Version.Major > 5)
            //{
            //    NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_AWAYMODE_REQUIRED |
            //        NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED |
            //        NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
            //}

            Common.Settings.CCulture();
            Common.Settings.CreateFolder();

            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2); // Установите желаемое время задержки
            _timer.Tick += Timer_Tick;

            //this.SetStyle(ControlStyles.UserPaint |
            //              ControlStyles.AllPaintingInWmPaint |
            //              ControlStyles.ResizeRedraw |
            //              ControlStyles.ContainerControl |
            //              ControlStyles.OptimizedDoubleBuffer |
            //              //ControlStyles.Opaque |
            //              ControlStyles.SupportsTransparentBackColor
            //              , true);

            _nameCurrentFile = DataFile.LogsPath();

            if (IniFile.IsExists(DataFile.SettingsPath()))
            {
                _iniFile = new IniFile(DataFile.SettingsPath());
                //_iniFile.Write("DSNV1", "1");
                //_iniFile.Write("DSNV2", "1");
                //_iniFile.Write("DSNV3", "1");
                //_iniFile.Write("DTVV1", "1");
                //_iniFile.Write("DTVV2", "1");
                //_iniFile.Write("DAD", "1");
                //_iniFile.Write("DVGO", "1");
                //_iniFile.Write("DMDV", "1");
            }
            else
            {
                using (File.Create(DataFile.SettingsPath()))
                {
                    // Using блок автоматически закроет FileStream
                }
                _iniFile = new IniFile(DataFile.SettingsPath());
                _iniFile.Write("DSNV1", "1");
                _iniFile.Write("DSNV2", "1");
                _iniFile.Write("DSNV3", "1");
                _iniFile.Write("DTVV", "1");
                //_iniFile.Write("DTVV2", "1");
                _iniFile.Write("DAD1", "1");
                _iniFile.Write("DAD2", "1");
                _iniFile.Write("DVGO", "1");
                _iniFile.Write("DMDV", "1");
                _iniFile.Write("HEIGHT", "0");
                _iniFile.Write("NAVIGATION", "0");
            }

            _iniFile.Read("DSNV1");
            _config.SetData(new List<string>{ _iniFile.Read("DSNV1"), _iniFile.Read("DSNV2"), _iniFile.Read("DSNV3"),
                _iniFile.Read("DTVV"), _iniFile.Read("DAD1"), _iniFile.Read("DAD2"), _iniFile.Read("DVGO"),  _iniFile.Read("DMDV"),
                _iniFile.Read("HEIGHT"), _iniFile.Read("NAVIGATION")});
            _customWindPanel1.SetConf(_config);
            ViewNow = (int)_typeWindow.customWindPanel1;

            richTextBoxMessage.MouseDoubleClick += richTextboxClear_Click;
            richTextBoxMessage.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            //_dt.Columns.Add("DateTime", typeof(DateTime));
            //_dt.Columns.Add("Temperature", typeof(int));


            //for(int i = 0; i < 1000; i++)
            //{
            //    DataRow row = _dt.NewRow();
            //    row["DateTime"] = new DateTime(1999, 1, 1, 0, 0, 0);
            //    row["Temperature"] = -999;
            //    _dt.Rows.Add(row);
            //}
            //customChart1.Dt = _dt;
            //customChart1.AllDtPointsToChart("Temperature");
            //panelCustomChart.Visibility = Visibility.Visible;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataDelegates.EventHandlerStr = new DataDelegates.MyEventStr(ShowDataTextBox);

            _serial.OpenPort();
            DataDelegates.WriteFHandlerStr = new DataDelegates.WriteFEventStr(WriteFile);
            DataDelegates.EventHandlerStrParam = new DataDelegates.MyEventStrParam(_customWindPanel1.ShowDataTablo);

            DB.StartDb();
            panelCustomChart.Visibility = Visibility.Hidden;
        }

        #region RichTextBox Терминал
        public void ShowDataTextBox(string str)
        {
            try
            {
                Dispatcher.BeginInvoke((MethodInvoker)delegate
                {
                    if (ViewNow == (int)_typeWindow.customTerminal)
                    {
                        richTextboxClear();
                        richTextBoxMessage.AppendText(str);
                    }
                    // richTextBoxMessage.AppendText(str);

                    //if (isScrollToEnd)
                    //    richTextBoxMessage.ScrollToEnd();

                    //if (richTextBoxMessage.Document.Blocks.Count > 200) //200 Сообщений в терминальном отображении
                    //    richTextboxClear();
                });
            }
            catch { }
        }

        public void richTextboxClear_Click(object sender, MouseButtonEventArgs e)
        {
            richTextboxClear();
        }

        public void richTextboxClear()
        {
            richTextBoxMessage.Document.Blocks.Clear();
        }

        #endregion

        public void WriteFile(string param)
        {
            if ((DateTime.Now.Minute) % 10 == 0 && (DateTime.Now.Second == 0))   //Записываем в файл каждые 10 минут
            {
                try
                {
                    Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {

                        //for (int i = 1; i < 91; i++)
                        //{
                        //    DateTime currentTime = new DateTime(2024, 8, 1);
                        //    currentTime = currentTime.AddDays(i);
                        //    _nameCurrentFile = $"C:\\Users\\dachtojtakoe\\Documents\\Capriz\\Log-{currentTime.ToString("dd.MM.yyyy HH.mm.ss")}.txt";
                        //    DataFile.WriteDataToFileALot(_nameCurrentFile, param, i);
                        //}
                        //for (int i = 1; i < 92; i++)
                        //{
                        //    DB.WriteDataToDBALot(param, i);
                        //}
                        try
                        {
                            DataFile.WriteDataToFile(_nameCurrentFile, param);
                        }
                        catch
                        {
                            _nameCurrentFile = DataFile.LogsPath();
                            DataFile.WriteDataToFile(_nameCurrentFile, param);
                        }
                        DB.WriteDataToDB(param);
                    });
                }
                catch { }
            }
        }

        private void HideAllPanels()
        {
            if (_customWindPanel1.Visibility == Visibility.Visible)
            {
                _customWindPanel1.Visibility = Visibility.Hidden;
            }
            if (customCalendar1.Visibility == Visibility.Visible)
            {
                customCalendar1.Visibility = Visibility.Hidden;
            }
            if (customGrid1.Visibility == Visibility.Visible)
            {
                customGrid1.Visibility = Visibility.Hidden;
            }
            if (panelCustomGrid.Visibility == Visibility.Visible)
            {
                panelCustomGrid.Visibility = Visibility.Hidden;
            }
            if (panelDates.Visibility == Visibility.Visible)
            {
                panelDates.Visibility = Visibility.Hidden;
            }
            if (customChart1.Visibility == Visibility.Visible)
            {
                //customChart1.Visibility = Visibility.Hidden;
            }
            if (panelCustomChart.Visibility == Visibility.Visible)
            {
                panelCustomChart.Visibility = Visibility.Hidden;
            }
            if (dateTimePanel.Visibility == Visibility.Visible)
            {
                dateTimePanel.Visibility = Visibility.Hidden;
            }
            if (richTextBoxMessage.Visibility == Visibility.Visible)
            {
                richTextBoxMessage.Visibility = Visibility.Hidden;
            }
            if (panelSettings.Visibility == Visibility.Visible)
            {
                panelSettings.Visibility = Visibility.Hidden;
            }
        }


        private void OpenDataFile()
        {
            Process[] explorers = Process.GetProcessesByName("explorer");
            foreach (Process explorer in explorers)
            {
                try
                {
                    explorer.Kill();
                    explorer.WaitForExit(); // Ожидать завершения процесса
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, если процесс не может быть закрыт
                    Console.WriteLine($"Ошибка при закрытии explorer.exe: {ex.Message}");
                }
            }
            Process.Start("explorer.exe", DataFile.CurrentDirectory());
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.ShowDialog();


            //string fileName = DataFile.GetFileName();
            //if (fileName != null && fileName != "")
            //{
            //    _dt = DataFile.DataToDataTable(DataFile.DataToList(DataFile.ReadDataFromFile(fileName)));
            //    lblText.TextAlignment = TextAlignment.Left;
            //    if (_dt.Rows.Count < 1)
            //    {
            //        lblText.Text = "Текущий файл: " + Environment.NewLine + fileName
            //            + Environment.NewLine + "имеет неподходящую в данном случае"
            //            + Environment.NewLine + "структуру файла или поврежден!";
            //    }
            //    else
            //        lblText.Text = "Текущий файл:" + Environment.NewLine + fileName + " успешно загружен!";

            //    ShowHideInfoPanel();
            //}
        }

        private void ShowChartPanel()
        {
            int oldView = ViewNow;
            ViewNow = (int)_typeWindow.customChart1;

            string firstRowDateTime = DB.GetFirstDate(ChartNow) == null ? null : DB.GetFirstDate(ChartNow);
            string lastRowDateTime = DB.GetLastDate(ChartNow) == null ? null : DB.GetLastDate(ChartNow);

            if ((firstRowDateTime == null) || (lastRowDateTime == null))
            {
                HideAllPanels();
                _customWindPanel1.Visibility = Visibility.Visible;
                ViewNow = (int)_typeWindow.customWindPanel1;
                lblText.Text = "\r\nНет данных!";
                lblText.TextAlignment = TextAlignment.Center;
                ShowHideInfoPanel();
                //ViewNow = oldView;
                return;
            }

            DtMin = Convert.ToDateTime(firstRowDateTime);
            DtMax = Convert.ToDateTime(lastRowDateTime);

            HideAllPanels();
            ShowHideDatePanel(firstRowDateTime, lastRowDateTime);
            isPanelOpened = false;
        }

        private void ShowChartPanelOLD()
        {
            ViewNow = (int)_typeWindow.customChart1;
            Stopwatch stopwatch = new Stopwatch();


            stopwatch.Start();
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar, "*.txt");
            string dataFromAllFiles = "";


            foreach (var file in files)
                dataFromAllFiles += DataFile.ReadDataFromFile(file);
            _dt = DataFile.DataToDataTable(DataFile.DataToList(dataFromAllFiles));

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.Elapsed;
            System.Windows.Forms.MessageBox.Show("" + elapsedMilliseconds);


            customChart1.Dt = _dt;

            if ((_dt == null) || (_dt.Rows.Count < 1))
            {
                //OpenDataFile();
                //if ((_dt == null) || (_dt.Rows.Count < 1))
                //    customChart1.Dt = _dt;
                //else return;

                lblText.Text = "Нет данных!";
                lblText.TextAlignment = TextAlignment.Center;
                ShowHideInfoPanel();
                return;
            }

            HideAllPanels();
            ShowHideDatePanelOLD();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ShowHideInfoPanel();
        }

        private string GetRemoteDevice()
        {
            var temp = "";
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    temp += drive.Name;
                    return temp;
                }
            }
            if (temp == "")
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.Name != "C:\\")
                    {
                        temp += drive.Name;
                        return temp;
                    }
                }
            }
            return temp;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = false, bool overwrite = true)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Папка c данными  не существует или не может быть найдена: "
                    + sourceDirName);
            }
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwrite);//false
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #region GetMinMaxDT
        public string GetMinDateDt
        {
            get
            {
                _dt.DefaultView.Sort = "Date DESC, Time DESC";
                return Convert.ToDateTime(string.Concat(_dt.Rows[0].Field<string>("Date") + " " +
                            _dt.Rows[0].Field<string>("Time"))).ToString();
            }
        }
        public string GetMaxDateDt
        {
            get
            {
                _dt.DefaultView.Sort = "Date DESC, Time DESC";
                int dtRowsCount = _dt.Rows.Count - 1;
                return Convert.ToDateTime(string.Concat(_dt.Rows[dtRowsCount].Field<string>("Date") + " " +
                            _dt.Rows[dtRowsCount].Field<string>("Time"))).ToString();
            }
        }

        public DateTime GetMinDateDtDB
        {
            get
            {
                _dt.DefaultView.Sort = "DateTime DESC";
                return Convert.ToDateTime(_dt.Rows[0]["DateTime"]);
            }
        }
        public DateTime GetMaxDateDtDB
        {
            get
            {
                _dt.DefaultView.Sort = "DateTime DESC";
                int dtRowsCount = _dt.Rows.Count - 1;
                return Convert.ToDateTime(_dt.Rows[dtRowsCount]["DateTime"]);
            }
        }

        #endregion

        public void ShowHideDatePanelOLD()
        {
            if (dateTimePanel.Visibility == Visibility.Hidden)
            {
                dateTimePanel.Visibility = Visibility.Visible;
                btnDateFrom.BtnText = GetMinDateDt;
                btnDateTo.BtnText = GetMaxDateDt;
            }
        }


        public void ShowHideDatePanel(string minDate, string maxDate)
        {
            if (dateTimePanel.Visibility == Visibility.Hidden)
            {
                dateTimePanel.Visibility = Visibility.Visible;
                btnDateFrom.BtnText = minDate;
                btnDateTo.BtnText = maxDate;
            }
        }

        public void ShowHideInfoPanel()
        {
            if (!(panelInformation.Visibility == Visibility.Visible))
            {
                panelInformation.Visibility = Visibility.Visible;
                isPanelOpened = false;
            }
            else
            {
                panelInformation.Visibility = Visibility.Hidden;
                isPanelOpened = true;
            }
        }

        private void ShowGridPanel()
        {
            int oldView = ViewNow;
            ViewNow = (int)_typeWindow.customGrid1;

            string firstRowDateTime = DB.GetFirstDate() == null ? null : DB.GetFirstDate();
            string lastRowDateTime = DB.GetLastDate() == null ? null : DB.GetLastDate();

            if ((firstRowDateTime == null) || (lastRowDateTime == null))
            {
                _customWindPanel1.Visibility = Visibility.Visible;
                ViewNow = (int)_typeWindow.customWindPanel1;
                lblText.Text = "\r\nНет данных!";
                lblText.TextAlignment = TextAlignment.Center;
                ShowHideInfoPanel();
                //ViewNow = oldView;
                return;
            }

            DtMin = Convert.ToDateTime(firstRowDateTime);
            DtMax = Convert.ToDateTime(lastRowDateTime);

            HideAllPanels();
            ShowHideDatePanel(firstRowDateTime, lastRowDateTime);
            isPanelOpened = false;
        }

        #region ButtonTooltipsVisibility

        private void CustomButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ViewNow != ((int)_typeWindow.customGrid1) && (ViewNow != (int)_typeWindow.customChart1))
            {
                var button = sender as CustomControls.CustomButton;
                if (button != null)
                {
                    var toolTipName = button.Tag as string;
                    if (!string.IsNullOrEmpty(toolTipName))
                    {
                        var toolTip = this.FindName(toolTipName) as CustomControls.CustomToolTip;
                        if (toolTip != null)
                        {
                            _currentToolTip = toolTip;
                            _timer.Start();
                            toolTip.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void CustomButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var button = sender as CustomControls.CustomButton;
            if (button != null)
            {
                var toolTipName = button.Tag as string;
                if (!string.IsNullOrEmpty(toolTipName))
                {
                    var toolTip = this.FindName(toolTipName) as CustomControls.CustomToolTip;
                    if (toolTip != null)
                    {
                        _currentToolTip = toolTip;
                        _timer.Stop();
                        toolTip.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_currentToolTip != null)
            {
                _currentToolTip.Visibility = Visibility.Hidden;
                _timer.Stop();
                _currentToolTip = null;
            }
        }

        #endregion


        #region DateTimePanelButtons

        private void btnDateFrom_Click(object sender, EventArgs e)
        {
            customCalendar1.SetData(Convert.ToDateTime(btnDateFrom.BtnText));
            customCalendar1.CalledBy = 1;
            customCalendar1.Visibility = Visibility.Visible;
        }

        private void btnDateTo_Click(object sender, EventArgs e)
        {
            customCalendar1.SetData(Convert.ToDateTime(btnDateTo.BtnText));
            customCalendar1.CalledBy = 2;
            customCalendar1.Visibility = Visibility.Visible;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            dateTimePanel.Visibility = Visibility.Hidden;

            btnlblDateFrom.BtnText = btnDateFrom.BtnText;
            btnlblDateTo.BtnText = btnDateTo.BtnText;

            if (ViewNow == (int)_typeWindow.customGrid1)
                ShowGrid(btnDateFrom.BtnText, btnDateTo.BtnText);
            if (ViewNow == (int)_typeWindow.customChart1)
                ShowChart(btnDateFrom.BtnText, btnDateTo.BtnText);

            btnPeriodClicked = 0;
            SetButtonFirstColor(btn6H);
            SetButtonFirstColor(btn1D);
            SetButtonFirstColor(btn7D);
            SetButtonFirstColor(btn1M);

            isPanelOpened = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dateTimePanel.Visibility = Visibility.Hidden;
            _customWindPanel1.Visibility = Visibility.Visible;
            ViewNow = (int)_typeWindow.customWindPanel1;

            btnPeriodClicked = 0;
            SetButtonFirstColor(btn6H);
            SetButtonFirstColor(btn1D);
            SetButtonFirstColor(btn7D);
            SetButtonFirstColor(btn1M);

            isPanelOpened = true;
        }

        private void btn6H_Click(object sender, RoutedEventArgs e)
        {
            if (btnPeriodClicked != 1)
            {
                btnPeriodClicked = 1;
                DateTime dt = Convert.ToDateTime(btnDateTo.BtnText);
                dt = dt.AddHours(-6);
                btnDateFrom.BtnText = dt < DtMin ? DtMin.ToString() : dt.ToString();

                ChangeButtonBorderColor(btn6H, Colors.White);
                SetButtonFirstColor(btn1D);
                SetButtonFirstColor(btn7D);
                SetButtonFirstColor(btn1M);
            }
            else
            {
                btnDateFrom.BtnText = DtMin.ToString();
                SetButtonFirstColor(btn6H);
                btnPeriodClicked = 0;
            }
        }

        private void btn1D_Click(object sender, RoutedEventArgs e)
        {
            if (btnPeriodClicked != 2)
            {
                btnPeriodClicked = 2;

                DateTime dt = Convert.ToDateTime(btnDateTo.BtnText);
                dt = dt.AddDays(-1);
                btnDateFrom.BtnText = dt < DtMin ? DtMin.ToString() : dt.ToString();

                ChangeButtonBorderColor(btn1D, Colors.White);
                SetButtonFirstColor(btn6H);
                SetButtonFirstColor(btn7D);
                SetButtonFirstColor(btn1M);
            }
            else
            {
                btnDateFrom.BtnText = DtMin.ToString();
                SetButtonFirstColor(btn1D);
                btnPeriodClicked = 0;
            }
        }

        private void btn7D_Click(object sender, RoutedEventArgs e)
        {
            if (btnPeriodClicked != 3)
            {
                btnPeriodClicked = 3;

                DateTime dt = Convert.ToDateTime(btnDateTo.BtnText);
                dt = dt.AddDays(-7);
                btnDateFrom.BtnText = dt < DtMin ? DtMin.ToString() : dt.ToString();

                ChangeButtonBorderColor(btn7D, Colors.White);
                SetButtonFirstColor(btn6H);
                SetButtonFirstColor(btn1D);
                SetButtonFirstColor(btn1M);
            }
            else
            {
                btnDateFrom.BtnText = DtMin.ToString();
                SetButtonFirstColor(btn7D);
                btnPeriodClicked = 0;
            }
        }

        private void btn1M_Click(object sender, RoutedEventArgs e)
        {
            if (btnPeriodClicked != 4)
            {
                btnPeriodClicked = 4;

                DateTime dt = Convert.ToDateTime(btnDateTo.BtnText);
                dt = dt.AddMonths(-1);
                btnDateFrom.BtnText = dt < DtMin ? DtMin.ToString() : dt.ToString();

                ChangeButtonBorderColor(btn1M, Colors.White);
                SetButtonFirstColor(btn6H);
                SetButtonFirstColor(btn1D);
                SetButtonFirstColor(btn7D);
            }
            else
            {
                btnDateFrom.BtnText = DtMin.ToString();
                SetButtonFirstColor(btn1M);
                btnPeriodClicked = 0;
            }

        }

        private void ChangeButtonBorderColor(System.Windows.Controls.Button button, Color newColor)
        {
            if (button.Template.FindName("Border", button) is Border border)
            {
                border.BorderBrush = new SolidColorBrush(newColor);
            }
        }

        private void SetButtonFirstColor(System.Windows.Controls.Button button)
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

        #endregion

        public void ShowGrid(string DateFrom, string DateTo)
        {
            _dt = DB.GetDataFiltered(Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd HH:mm:ss"));
            customGrid1.Dt = _dt;

            panelCustomGrid.Visibility = Visibility.Visible;
            customGrid1.Visibility = Visibility.Visible;
            panelDates.Visibility = Visibility.Visible;

            customGrid1.ShowDataGridFilter();
            lblCount.Text = "Количество строк: " + customGrid1.GetCountRecords;
        }


        public void ShowChartOLD(string DateFrom, string DateTo)
        {
            panelCustomChart.Visibility = Visibility.Visible;
            customChart1.Visibility = Visibility.Visible;
            panelDates.Visibility = Visibility.Visible;

            customChart1.AllDtPointsToChartOLD(ChartNow, new string[] { DateFrom, DateTo });
            lblCount.Text = "Количество записей: " + customChart1.PointsCount;
            ViewNow = (int)_typeWindow.customChart1;
        }

        public void ShowChart(string DateFrom, string DateTo)
        {
            _dt = DB.GetDataFiltered(Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd HH:mm:ss"), ChartNow);
            customChart1.Dt = _dt;

            customChart1.AllDtPointsToChart(ChartNow);

            panelCustomChart.Visibility = Visibility.Visible;
            //customChart1.Visibility = Visibility.Visible;
            panelDates.Visibility = Visibility.Visible;


            lblCount.Text = "Количество точек: " + customChart1.PointsCount;
            ViewNow = (int)_typeWindow.customChart1;
        }

        private void btnlblDateFrom_Click(object sender, EventArgs e)
        {
            if (ViewNow == (int)_typeWindow.customChart1)
            {
                panelCustomChart.Visibility = Visibility.Hidden;
                //customChart1.Visibility = Visibility.Hidden;
            }
            else if (ViewNow == (int)_typeWindow.customGrid1)
            {
                panelCustomGrid.Visibility = Visibility.Hidden;
                //customGrid1.Visibility = Visibility.Hidden;
            }
            customCalendar1.SetData(Convert.ToDateTime(btnlblDateFrom.BtnText));
            customCalendar1.CalledBy = 3;
            customCalendar1.Visibility = Visibility.Visible;
            isPanelOpened = false;
        }

        private void btnlblDateTo_Click(object sender, EventArgs e)
        {
            if (ViewNow == (int)_typeWindow.customChart1)
            {
                panelCustomChart.Visibility = Visibility.Hidden;
                //customChart1.Visibility = Visibility.Hidden;
            }
            else if (ViewNow == (int)_typeWindow.customGrid1)
            {
                panelCustomGrid.Visibility = Visibility.Hidden;
                //customGrid1.Visibility = Visibility.Hidden;
            }
            customCalendar1.SetData(Convert.ToDateTime(btnlblDateTo.BtnText));
            customCalendar1.CalledBy = 4;
            customCalendar1.Visibility = Visibility.Visible;
            isPanelOpened = false;
        }

        #region MenuButtons

        private void btnTablo_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                HideAllPanels();
                _customWindPanel1.Visibility = Visibility.Visible;
                ViewNow = (int)_typeWindow.customWindPanel1;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                HideAllPanels();
                _customWindPanel1.Visibility = Visibility.Visible;
                ViewNow = (int)_typeWindow.customWindPanel1;
                OpenDataFile();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                HideAllPanels();
                _customWindPanel1.Visibility = Visibility.Visible;
                ViewNow = (int)_typeWindow.customWindPanel1;

                string sourceDirName = Directory.GetCurrentDirectory();
                string nameDir = sourceDirName.Substring(sourceDirName.LastIndexOf('\\') + 1);

                string destDirName = GetRemoteDevice();

                if (destDirName != "")
                {
                    try
                    {
                        DirectoryCopy(sourceDirName, destDirName + nameDir, true);
                        lblText.Text = "Копирование папки с данными завершено\r\nПапка с данными расположена в " + destDirName + nameDir;
                        lblText.TextAlignment = TextAlignment.Center;
                        ShowHideInfoPanel();
                    }
                    catch
                    {
                        lblText.Text = "На данный внешний диск не может быть осуществлена запись\r\n\r\nВставьте другое USB устройство!";
                        lblText.TextAlignment = TextAlignment.Center;
                        ShowHideInfoPanel();
                    }
                }
                else
                {
                    lblText.Text = "Внешнего диска в системе не найдено или на него не может быть осуществлена запись\r\nВставьте другое USB устройство!";
                    lblText.TextAlignment = TextAlignment.Center;
                    ShowHideInfoPanel();
                }
            }
        }

        private void btnGrid_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
                ShowGridPanel();
        }

        private void btnChT_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                ChartNow = "Temperature";
                ShowChartPanel();
            }
        }

        private void btnChH_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                ChartNow = "Humidity";
                ShowChartPanel();
            }
        }

        private void btnChW_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                ChartNow = "Speed_I";
                ShowChartPanel();
            }
        }

        private void btnChP_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                ChartNow = "PressureGPa";
                ShowChartPanel();
            }
        }

        private void btnChOp_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                ChartNow = "Visibility10";
                ShowChartPanel();
            }
        }


        private void btn_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                ViewNow = (int)_typeWindow.customTerminal;
                if (richTextBoxMessage.Visibility == Visibility.Visible)
                {
                    isScrollToEnd = isScrollToEnd ? false : true;
                }
                else
                {
                    HideAllPanels();
                    richTextBoxMessage.Visibility = Visibility.Visible;
                    isScrollToEnd = true;
                }
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                if (ViewNow == (int)_typeWindow.customChart1)
                {
                    customChart1.SaveChart(ChartNow);
                }
                else
                {
                    if (ViewNow == (int)_typeWindow.customGrid1)
                    {
                        HideAllPanels();
                        _customWindPanel1.Visibility = Visibility.Visible;
                        ViewNow = (int)_typeWindow.customWindPanel1;
                    }
                    lblText.Text = "\r\nВ данной версии программы предусмотрено\r\n только сохнанение графиков.";
                    lblText.TextAlignment = TextAlignment.Center;
                    ShowHideInfoPanel();
                }
            }
        }


        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    if (isPanelOpened)
        //    {
        //        if (ViewNow == (int)_typeWindow.customChart1)
        //        {
        //            var elementToPrint = customChart1; // Например, Grid с именем MyGrid

        //            int cropX = 0; // Начальная координата X
        //            int cropY = 0; // Начальная координата Y
        //            int cropWidth = 680; // Ширина обрезки
        //            int cropHeight = 540; // Высота обрезки

        //            // Создаем VisualBrush
        //            // Создаем DrawingVisual для рисования обрезанной части

        //            var drawingVisual = new DrawingVisual();
        //            using (var drawingContext = drawingVisual.RenderOpen())
        //            {
        //                var visualBrush = new VisualBrush(elementToPrint);
        //                drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(-cropX, -cropY), new Size(elementToPrint.ActualWidth, elementToPrint.ActualHeight)));
        //            }

        //            // Создаем VisualBrush из DrawingVisual
        //            var croppedVisualBrush = new VisualBrush(drawingVisual)
        //            {
        //                Viewbox = new Rect(cropX, cropY, cropWidth, cropHeight),
        //                ViewboxUnits = BrushMappingMode.Absolute,
        //                Stretch = Stretch.None
        //            };

        //            // Открываем диалог печати
        //            var printDialog = new PrintDialog();
        //            if (printDialog.ShowDialog() == true)
        //            {
        //                // Создаем документ для печати
        //                var fixedDocument = new FixedDocument();
        //                var pageContent = new PageContent();
        //                var fixedPage = new FixedPage();

        //                // Рисуем часть окна на FixedPage
        //                //var visualBrush = new VisualBrush(elementToPrint);
        //                fixedPage.Width = 680;
        //                fixedPage.Height = 540;
        //                fixedPage.Background = croppedVisualBrush;
        //                // Добавляем FixedPage в PageContent
        //                ((IAddChild)pageContent).AddChild(fixedPage);
        //                fixedDocument.Pages.Add(pageContent);

        //                // Отправляем документ на печать
        //                printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Printing Part of Window");
        //            }
        //        }
        //        else
        //        {
        //            if (ViewNow == (int)_typeWindow.customGrid1)
        //            {
        //                HideAllPanels();
        //                _customWindPanel1.Visibility = Visibility.Visible;
        //                ViewNow = (int)_typeWindow.customWindPanel1;
        //            }
        //            lblText.Text = "В данной версии программы предусмотрена\r\n только печать графиков.";
        //            lblText.TextAlignment = TextAlignment.Center;
        //            ShowHideInfoPanel();
        //        }
        //    }
        //}

        private void btnVers_Click(object sender, EventArgs e)
        {
            if (isPanelOpened)
            {
                HideAllPanels();

                _config.SetData(new List<string>{ _iniFile.Read("DSNV1"), _iniFile.Read("DSNV2"), _iniFile.Read("DSNV3"),
                _iniFile.Read("DTVV"), _iniFile.Read("DAD1"), _iniFile.Read("DAD2"), _iniFile.Read("DVGO"),  _iniFile.Read("DMDV"),
                _iniFile.Read("HEIGHT"), _iniFile.Read("NAVIGATION")});

                DSNV1Check.IsChecked = _config.DSNV1 == "1" ? true : false;
                DSNV2Check.IsChecked = _config.DSNV2 == "1" ? true : false;
                DSNV3Check.IsChecked = _config.DSNV3 == "1" ? true : false;
                DTVVCheck.IsChecked = _config.DTVV == "1" ? true : false;
                DAD1Check.IsChecked = _config.DAD1 == "1" ? true : false;
                DAD2Check.IsChecked = _config.DAD2 == "1" ? true : false;
                DVGOCheck.IsChecked = _config.DVGO == "1" ? true : false;
                DMDVCheck.IsChecked = _config.DMDV == "1" ? true : false;

                panelSettings.Visibility = Visibility.Visible;
                isPanelOpened = false;
            }
        }

        private void btnSetOk_Click(object sender, EventArgs e)
        {
            _iniFile = new IniFile(DataFile.SettingsPath());
            string TTF = ""; //TextToFile
            List<string> newConfig = new List<string>();

            TTF = (bool)DSNV1Check.IsChecked ? "1" : "0";
            _iniFile.Write("DSNV1", TTF);
            newConfig.Add(TTF);

            TTF = (bool)DSNV2Check.IsChecked ? "1" : "0";
            _iniFile.Write("DSNV2", TTF);
            newConfig.Add(TTF);

            TTF = (bool)DSNV3Check.IsChecked ? "1" : "0";
            _iniFile.Write("DSNV3", TTF);
            newConfig.Add(TTF);

            TTF = (bool)DTVVCheck.IsChecked ? "1" : "0";
            _iniFile.Write("DTVV", TTF);
            newConfig.Add(TTF);

            TTF = (bool)DAD1Check.IsChecked ? "1" : "0";
            _iniFile.Write("DAD1", TTF);
            newConfig.Add(TTF);

            TTF = (bool)DAD2Check.IsChecked ? "1" : "0";
            _iniFile.Write("DAD2", TTF);
            newConfig.Add(TTF);

            TTF = (bool)DVGOCheck.IsChecked ? "1" : "0";
            _iniFile.Write("DVGO", TTF);
            newConfig.Add(TTF);

            TTF = (bool)DMDVCheck.IsChecked ? "1" : "0";
            _iniFile.Write("DMDV", TTF);
            newConfig.Add(TTF);

            newConfig.Add(_config.HEIGHT);
            newConfig.Add(_config.NAVIGATION);

            _config.SetData(newConfig);
            _customWindPanel1.SetConf(_config); //!!!!

            HideAllPanels();
            _customWindPanel1.Visibility = Visibility.Visible;
            ViewNow = (int)_typeWindow.customWindPanel1;
            isPanelOpened = true;
        }

        private void btnSetPresModif_Click(object sender, EventArgs e)
        {
            StationHeight.BtnText = _config.HEIGHT + " м";

            panelSetPressureModification.Visibility = Visibility.Visible;
        }

        private void btnSetNavigation_Click(object sender, EventArgs e)
        {
            if (_config.NAVIGATION == "1")
            {
                GPSRB.IsChecked = true;
            }
            else
            {
                LAGRB.IsChecked = true;
            }
            panelSetNavigation.Visibility = Visibility.Visible;
        }

        private void btnApprovePresModif_Click(object sender, EventArgs e)
        {
            panelSetPressureModification.Visibility = Visibility.Hidden;

            string TTF = double.Parse(StationHeight.BtnText.Substring(0, StationHeight.BtnText.Length - 2)).ToString();
            _iniFile.Write("HEIGHT", TTF);
            _config.HEIGHT = TTF;

            _customWindPanel1.SetConf(_config);
        }

        private void odd1m_Click(object sender, RoutedEventArgs e)
        {
            double height = double.Parse(StationHeight.BtnText.Substring(0, StationHeight.BtnText.Length - 2));
            if (height - 1 >= 0)
            {
                StationHeight.BtnText = (height - 1).ToString() + " м";
            }
        }

        private void odd01m_Click(object sender, RoutedEventArgs e)
        {
            double height = double.Parse(StationHeight.BtnText.Substring(0, StationHeight.BtnText.Length - 2));
            if (height - 0.1 >= 0)
            {
                StationHeight.BtnText = (height - 0.1).ToString() + " м";
            }
        }

        private void add01m_Click(object sender, RoutedEventArgs e)
        {
            double height = double.Parse(StationHeight.BtnText.Substring(0, StationHeight.BtnText.Length - 2));
            StationHeight.BtnText = (height + 0.1).ToString() + " м";
        }

        private void add1m_Click(object sender, RoutedEventArgs e)
        {
            double height = double.Parse(StationHeight.BtnText.Substring(0, StationHeight.BtnText.Length - 2));
            StationHeight.BtnText = (height + 1).ToString() + " м";
        }

        private void btnCancelPresModif_Click(object sender, EventArgs e)
        {
            panelSetPressureModification.Visibility = Visibility.Hidden;
        }

        private void btnSetNavigationOk_Click(object sender, EventArgs e)
        {
            panelSetNavigation.Visibility = Visibility.Hidden;
            string TTF;
            if (LAGRB.IsChecked == true)
            {
                TTF = "0";
            }
            else
            {
                TTF = "1";
            }

            _iniFile.Write("NAVIGATION", TTF);
            _config.NAVIGATION = TTF;

            _customWindPanel1.SetConf(_config);
        }

        #endregion
    }
}

