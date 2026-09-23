using System;
using Capriz_WPF.Data;
using System.Collections.Generic;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Threading;
using Capriz_WPF.Common;

namespace Capriz_WPF.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomWindPanel.xaml
    /// </summary>
    public partial class CustomWindPanel : System.Windows.Controls.UserControl
    {
        byte cleanData = 0;
        string status = "";
        string toolTipText = "";
        int currentDataSource = 0;

        int CurrentDataSource
        {
            get { return currentDataSource; }
            set { currentDataSource = value > 2 ? 0 : value; }
        }



        List<Data.Data> msg = new List<Data.Data>();
        Configuration conf;
        private DispatcherTimer timerClock;

        public CustomWindPanel()
        {
            InitializeComponent();
            CleanDataTablo();
            timerClock = new DispatcherTimer();
            timerClock.Interval = TimeSpan.FromSeconds(1);
            timerClock.Tick += timerClock_Tick;
            timerClock.Start();
        }

        public void SetConf(Configuration _conf)
        {
            conf = _conf;
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            customBottomDataPanel1.SetDateTime(new List<string> { (dt).ToString(@"dd.MM.yyyy"), (dt).ToString(@" HH:mm:ss") });
            cleanData++;
            if (status != "/")
                if (cleanData >= 5) CleanDataTablo();
        }

        void changePanels()
        {
            if (customWindDataNew1.Visibility == Visibility.Visible)
            {
                customWindDataNew1.Visibility = Visibility.Hidden;
                customDataPanel1.Visibility = Visibility.Visible;
                customAdditionalDataPanel1.Visibility = Visibility.Hidden;
            }
            else if (customDataPanel1.Visibility == Visibility.Visible)
            {
                customWindDataNew1.Visibility = Visibility.Hidden;
                customDataPanel1.Visibility = Visibility.Hidden;
                customAdditionalDataPanel1.Visibility = Visibility.Visible;
            }
            else if (customAdditionalDataPanel1.Visibility == Visibility.Visible)
            {
                customWindDataNew1.Visibility = Visibility.Visible;
                customDataPanel1.Visibility = Visibility.Hidden;
                customAdditionalDataPanel1.Visibility = Visibility.Hidden;
            }
        }

        private void customWindDataNew1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            changePanels();
        }

        private void customDataPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            changePanels();
        }

        private void customAdditionalData1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            changePanels();
        }


        public void CleanDataTablo()
        {
            try
            {
                Dispatcher.BeginInvoke((MethodInvoker)delegate
                {
                    customBottomDataPanel1.SetDataToSost(SetStatus(null));

                    customWindDataNew1.ClearFields();

                    customDataPanel1.ClearFields();

                    customBottomDataPanel1.ClearShipFields();

                    customRoundWindPanel1.ValueSpeed = ("Н.Д.");
                    customRoundWindPanel2.ValueSpeed = ("Н.Д.");
                    customRoundWindPanel1.ValueDir = ("Н.Д.");
                    customRoundWindPanel2.ValueDir = ("Н.Д.");
                    customRoundWindPanel2.ValueCurs = ("Н.Д.");
                    customRoundWindPanel1.ValueCurs = ("0");

                    customRoundWindPanel1.ValueDir_2mid = ("Н.Д.");
                    customRoundWindPanel1.ValueDir_2min = ("Н.Д.");
                    customRoundWindPanel1.ValueDir_2max = ("Н.Д.");
                    customRoundWindPanel1.ValueDir_10mid = ("Н.Д.");
                    customRoundWindPanel1.ValueDir_10min = ("Н.Д.");
                    customRoundWindPanel1.ValueDir_10max = ("Н.Д.");

                    customRoundWindPanel2.ValueDir_2mid = ("Н.Д.");
                    customRoundWindPanel2.ValueDir_2min = ("Н.Д.");
                    customRoundWindPanel2.ValueDir_2max = ("Н.Д.");
                    customRoundWindPanel2.ValueDir_10mid = ("Н.Д.");
                    customRoundWindPanel2.ValueDir_10min = ("Н.Д.");
                    customRoundWindPanel2.ValueDir_10max = ("Н.Д.");

                    cleanData = 6;

                });
            }
            catch { }
        }

        public void ShowDataTablo(string str)
        {
            try
            {
                Dispatcher.BeginInvoke((MethodInvoker)delegate
                {
                    msg = DataToTable.DataToList(str);
                    foreach (var data in msg)
                    {
                        cleanData = 0;

                        switch (CurrentDataSource)
                        {
                            case 0:
                                customRoundWindPanel1.SetData(new List<string>() { data.Speed_K.Trim(), data.Direction_K.Trim(), "0", data.Direction_2Kmid.Trim(), data.Direction_2Kmin.Trim(), data.Direction_2Kmax.Trim(), data.Direction_10Kmid.Trim(), data.Direction_10Kmin.Trim(), data.Direction_10Kmax.Trim() });
                                customRoundWindPanel2.SetData(new List<string>() { data.Speed_I.Trim(), data.Direction_I.Trim(), data.CourseShip.Trim(), data.Direction_2Imid.Trim(), data.Direction_2Imin.Trim(), data.Direction_2Imax.Trim(), data.Direction_10Imid.Trim(), data.Direction_10Imin.Trim(), data.Direction_10Imax.Trim() });
                                customWindDataNew1.SetDataToFields(new List<string>() { data.Speed_2Kmin, data.Speed_10Kmin, data.Speed_2Kmid, data.Speed_10Kmid, data.Speed_2Kmax, data.Speed_10Kmax, data.Speed_2Imin, data.Speed_10Imin, data.Speed_2Imid, data.Speed_10Imid, data.Speed_2Imax, data.Speed_10Imax });
                                break;
                            case 1:
                                customRoundWindPanel1.SetData(new List<string>() { data.Speed_K2.Trim(), data.Direction_K2.Trim(), "0", data.Direction_2Kmid_2.Trim(), data.Direction_2Kmin_2.Trim(), data.Direction_2Kmax_2.Trim(), data.Direction_10Kmid_2.Trim(), data.Direction_10Kmin_2.Trim(), data.Direction_10Kmax_2.Trim() });
                                customRoundWindPanel2.SetData(new List<string>() { data.Speed_I2.Trim(), data.Direction_I2.Trim(), data.CourseShip.Trim(), data.Direction_2Imid_2.Trim(), data.Direction_2Imin_2.Trim(), data.Direction_2Imax_2.Trim(), data.Direction_10Imid_2.Trim(), data.Direction_10Imin_2.Trim(), data.Direction_10Imax_2.Trim() });
                                customWindDataNew1.SetDataToFields(new List<string>() { data.Speed_2Kmin_2, data.Speed_10Kmin_2, data.Speed_2Kmid_2, data.Speed_10Kmid_2, data.Speed_2Kmax_2, data.Speed_10Kmax_2, data.Speed_2Imin_2, data.Speed_10Imin_2, data.Speed_2Imid_2, data.Speed_10Imid_2, data.Speed_2Imax_2, data.Speed_10Imax_2 });
                                break;
                            case 2:
                                customRoundWindPanel1.SetData(new List<string>() { data.Speed_Kw.Trim(), data.Direction_Kw.Trim(), "0", data.Direction_2Kmid_w.Trim(), data.Direction_2Kmin_w.Trim(), data.Direction_2Kmax_w.Trim(), data.Direction_10Kmid_w.Trim(), data.Direction_10Kmin_w.Trim(), data.Direction_10Kmax_w.Trim() });
                                customRoundWindPanel2.SetData(new List<string>() { data.Speed_Iw.Trim(), data.Direction_Iw.Trim(), data.CourseShip.Trim(), data.Direction_2Imid_w.Trim(), data.Direction_2Imin_w.Trim(), data.Direction_2Imax_w.Trim(), data.Direction_10Imid_w.Trim(), data.Direction_10Imin_w.Trim(), data.Direction_10Imax_w.Trim() });
                                customWindDataNew1.SetDataToFields(new List<string>() { data.Speed_2Kmin_w, data.Speed_10Kmin_w, data.Speed_2Kmid_w, data.Speed_10Kmid_w, data.Speed_2Kmax_w, data.Speed_10Kmax_w, data.Speed_2Imin_w, data.Speed_10Imin_w, data.Speed_2Imid_w, data.Speed_10Imid_w, data.Speed_2Imax_w, data.Speed_10Imax_w });
                                break;
                        }

                        customDataPanel1.SetDataToFields(new List<string>() { data.Temperature, data.Humidity, data.PressureRtSt, data.PressureGPa, data.BarTend, data.Trend, data.AmountClouds, data.Visibility1, data.Visibility10, data.NGO1, data.NGO2, data.NGO3 });

                        string lat = (string.IsNullOrEmpty(data.LatDeg) || data.LatDeg == "Н.Д.")
                            ? "Н.Д."
                            : $"{data.LatDeg}° {data.LatMin}' {data.LatSec}\" {data.LatNS}";

                        string lon = (string.IsNullOrEmpty(data.LonDeg) || data.LonDeg == "Н.Д.")
                            ? "Н.Д."
                            : $"{data.LonDeg}° {data.LonMin}' {data.LonSec}\" {data.LonEW}";


                        customAdditionalDataPanel1.SetDataToFields(new List<string>()
                            {
                                data.AmountPrecipitation,
                                lat,
                                lon,
                                data.Hm0,
                                data.Hmax
                            });
                        if (data.ShipSpeed != "Н.Д.") data.ShipSpeed = (Convert.ToString(Math.Round(double.Parse(data.ShipSpeed) * 1.94384449244, 1)));

                        customBottomDataPanel1.SetDataToFields(new List<string>() { data.CourseShip, data.ShipSpeed });

                        customBottomDataPanel1.SetDataToSost(SetStatus(data));
                    }
                });
            }
            catch
            {
                System.Windows.MessageBox.Show("error");
            }

        }

        private List<string> SetStatus(Data.Data data)
        {
            status = "1";
            toolTipText = "";
            if (data == null)
            {
                status = "/";
                toolTipText = "Нет данных с БПР-1";
                return new List<string> { status, toolTipText };
            }

            toolTipText += "Статус датчика температуры ДМП-1 №1: " + data.StatusTemp1 + "\r\n";
            toolTipText += "Статус датчика температуры ДМП-1 №2: " + data.StatusTemp2 + "\r\n";
            toolTipText += "Статус датчика влажности ДМП-1 №1: " + data.StatusHum1 + "\r\n";
            toolTipText += "Статус датчика влажности ДМП-1 №2: " + data.StatusHum2 + "\r\n";
            toolTipText += "Статус датчика ветра ДМП-1 №1: " + data.StatusWind1 + "\r\n";
            toolTipText += "Статус датчика ветра ДМП-1 №2: " + data.StatusWind2 + "\r\n";
            toolTipText += "Статус датчика ветра WMT-702: " + data.StatusWindWMT + "\r\n";
            toolTipText += "Статус датчика атмосферного давления  ДМП-1 №1: " + data.StatusPressure1 + "\r\n";
            toolTipText += "Статус датчика атмосферного давления ДМП-1 №2: " + data.StatusPressure2 + "\r\n";
            toolTipText += "Ошибки по прибору SKYDEX 15: " + data.StatusSKYDEX + "\r\n";
            toolTipText += "Количество слоёв облаков: " + data.AmountClouds + "\r\n";
            toolTipText += "Ошибки по датчику ДМДВ: " + data.StatusDMDV + "\r\n";


            //if ((conf.DSNV1 == "1") && (conf.DSNV2 == "1"))
            //{
            //    if (data.StatusSpeed1 == "/" || data.StatusSpeed1 == "0") status = "0";
            //    toolTipText += (data.StatusSpeed1 == "/") ? "Отключен основной канал скорости ветра;\r\n" :
            //        (data.StatusSpeed1 == "0") ? "Авария по основному каналу скорости ветра;\r\n" : "";
            //    if (data.StatusDirect1 == "/" || data.StatusDirect1 == "0") status = "0";
            //    toolTipText += (data.StatusDirect1 == "/") ? "Отключен основной канал направления ветра;\r\n" :
            //        (data.StatusDirect1 == "0") ? "Авария по основному каналу направления ветра;\r\n" : "";

            //    if (data.StatusSpeed2 == "/" || data.StatusSpeed2 == "0") status = "0";
            //    toolTipText += (data.StatusSpeed2 == "/") ? "Отключен резервный канал скорости ветра;\r\n" :
            //        (data.StatusSpeed2 == "0") ? "Авария по резервному каналу скорости ветра;\r\n" : "";
            //    if (data.StatusDirect2 == "/" || data.StatusDirect2 == "0") status = "0";
            //    toolTipText += (data.StatusDirect2 == "/") ? "Отключен резервный канал направления ветра;\r\n" :
            //        (data.StatusDirect2 == "0") ? "Авария по резервному каналу направления ветра;\r\n" : "";
            //}

            //if (conf.DSNV3 == "1")
            //{
            //    if (data.StatusSpeedNasal == "/" || data.StatusSpeedNasal == "0") status = "0";
            //    toolTipText += (data.StatusSpeedNasal == "/") ? "Отключен канал скорости ветра ВПП;\r\n" :
            //        (data.StatusSpeedNasal == "0") ? "Авария по каналу скорости ветра ВПП;\r\n" : "";
            //    if (data.StatusDirectNasal == "/" || data.StatusDirectNasal == "0") status = "0";
            //    toolTipText += (data.StatusDirectNasal == "/") ? "Отключен канал направления ветра ВПП;\r\n" :
            //        (data.StatusDirectNasal == "0") ? "Авария по каналу направления ветра ВПП;\r\n" : "";
            //}

            //if ((conf.DSNV1 == "1") && (conf.DSNV2 == "0") && (conf.DSNV3 == "0"))
            //{
            //    if (data.StatusSpeed1 == "/" || data.StatusSpeed1 == "0") status = "0";
            //    toolTipText += (data.StatusSpeed1 == "/") ? "Отключен канал скорости ветра;\r\n" :
            //        (data.StatusSpeed1 == "0") ? "Авария по каналу скорости ветра;\r\n" : "";
            //    if (data.StatusDirect1 == "/" || data.StatusDirect1 == "0") status = "0";
            //    toolTipText += (data.StatusDirect1 == "/") ? "Отключен канал направления ветра;\r\n" :
            //        (data.StatusDirect1 == "0") ? "Авария по каналу направления ветра;\r\n" : "";
            //}

            //if ((conf.DTVV1 == "1") && (conf.DTVV2 == "1"))
            //{
            //    if (data.StatusTemp1 == "/" || data.StatusTemp1 == "0") status = "0";
            //    toolTipText += (data.StatusTemp1 == "/") ? "Отключен основной канал температуры;\r\n" :
            //        (data.StatusTemp1 == "0") ? "Авария по основному каналу температуры;\r\n" : "";
            //    if (data.StatusHum1 == "/" || data.StatusHum1 == "0") status = "0";
            //    toolTipText += (data.StatusHum1 == "/") ? "Отключен основной канал влажности;\r\n" :
            //        (data.StatusHum1 == "0") ? "Авария по основнову каналу влажности;\r\n" : "";

            //    if (data.StatusTemp2 == "/" || data.StatusTemp2 == "0") status = "0";
            //    toolTipText += (data.StatusTemp2 == "/") ? "Отключен резервный канал температуры;\r\n" :
            //        (data.StatusTemp2 == "0") ? "Авария по резервному каналу температуры;\r\n" : "";
            //    if (data.StatusHum2 == "/" || data.StatusHum2 == "0") status = "0";
            //    toolTipText += (data.StatusHum2 == "/") ? "Отключен резервный канал влажности;\r\n" :
            //        (data.StatusHum2 == "0") ? "Авария по резервному каналу влажности;\r\n" : "";
            //}

            //if ((conf.DTVV1 == "1") && (conf.DTVV2 == "0"))
            //{
            //    if (data.StatusTemp1 == "/" || data.StatusTemp1 == "0") status = "0";
            //    toolTipText += (data.StatusTemp1 == "/") ? "Отключен канал температуры ;\r\n" :
            //        (data.StatusTemp1 == "0") ? "Авария по каналу температуры;\r\n" : "";
            //    if (data.StatusHum1 == "/" || data.StatusHum1 == "0") status = "0";
            //    toolTipText += (data.StatusHum1 == "/") ? "Отключен канал влажности;\r\n" :
            //        (data.StatusHum1 == "0") ? "Авария по каналу влажности;\r\n" : "";
            //}

            //if (conf.DAD == "1")
            //{
            //    if (data.StatusPressure == "/" || data.StatusPressure == "0") status = "0";
            //    toolTipText += (data.StatusPressure == "/") ? "Отключен канал атмосферного давления;\r\n" :
            //        (data.StatusPressure == "0") ? "Авария по каналу атмосферного давления;\r\n" : "";
            //}

            //if (conf.DVGO == "1")
            //{
            //    if (data.StatusDVGO == "/" || data.StatusDVGO == "A" || data.StatusDVGO == "W") status = "0";
            //    toolTipText += (data.StatusDVGO == "/") ? "Отключен датчик верхней границы облаков;\r\n" :
            //        (data.StatusDVGO == "A") ? "Авария по датчику верхней границы облаков;\r\n" :
            //        (data.StatusDVGO == "W") ? "Тревога по датчику верхней границы облаков;\r\n" : "";
            //}

            //if (conf.DMDV == "1")
            //{
            //    if (data.StatusDMDV == "/" || data.StatusDMDV == "1" || data.StatusDMDV == "2" || data.StatusDMDV == "3" || data.StatusDMDV == "4") status = "0";
            //    toolTipText += (data.StatusDMDV == "/") ? "Отключен датчик метеорологической\r\nдальности видимости;\r\n" :
            //        (data.StatusDMDV == "1") ? "Ошибка оборудования по датчику\r\nметеорологической дальности видимости;\r\n" :
            //        (data.StatusDMDV == "2") ? "Предупреждение по оборудованию датчика\r\nметеорологической дальности видимости;\r\n" :
            //        (data.StatusDMDV == "3") ? "Тревога по обратному рассеянию датчика\r\nметеорологической дальности видимости;\r\n" :
            //        (data.StatusDMDV == "4") ? "Предупреждение по обратному рассеянию датчика\r\nметеорологической дальности видимости;\r\n" : "";
            //}


            if (data.ShipSpeed == "Н.Д.") status = "0";
            toolTipText += (data.ShipSpeed == "Н.Д.") ? "Нет скорости корабля;\r\n" : "";

            if (data.CourseShip == "Н.Д.") status = "0";
            toolTipText += (data.CourseShip == "Н.Д.") ? "Нет курса корабля;\r\n" : "";

            return new List<string> { status, toolTipText };
        }


        private void ChangeDataSource(object sender, EventArgs e)
        {
            CurrentDataSource++;
            switch (CurrentDataSource)
            {
                case 0:
                    ChangeDataSourceButton.BtnText = "ДМП№1";
                    break;
                case 1:
                    ChangeDataSourceButton.BtnText = "ДМП№2";
                    break;
                case 2:
                    ChangeDataSourceButton.BtnText = "WMT702";
                    break;
            }
            //ShowHideInfoPanel();
        }

    }
}
