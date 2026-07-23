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
            customDadDataPanel1.SetConfig(_conf);
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
            }
            else if (customDataPanel1.Visibility == Visibility.Visible)
            {
                customDataPanel1.Visibility = Visibility.Hidden;
                customDadDataPanel1.Visibility = Visibility.Visible;
            }
            else if (customDadDataPanel1.Visibility == Visibility.Visible)
            {
                customDadDataPanel1.Visibility = Visibility.Hidden;
                customWindDataNew1.Visibility = Visibility.Visible;
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

        private void customDadDataPanel1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

                    customDadDataPanel1.ClearFields();

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
                        customBottomDataPanel1.SetDataToSost(SetStatus(data));
                        cleanData = 0;

                        customWindDataNew1.SetDataToFields(new List<string>() { data.Speed_2Kmin, data.Speed_10Kmin, data.Speed_2Kmid, data.Speed_10Kmid, data.Speed_2Kmax, data.Speed_10Kmax, data.Speed_2Imin, data.Speed_10Imin, data.Speed_2Imid, data.Speed_10Imid, data.Speed_2Imax, data.Speed_10Imax });

                        int skydexIndex = -1;
                        try
                        {
                            skydexIndex = Convert.ToInt32(data.AmountClouds);
                        }
                        catch { }

                        try
                        {
                            if(data.AmountClouds == "Н.Д.")
                            {
                                data.NGO1 = "Н.Д.";
                                data.NGO2 = "Н.Д.";
                                data.NGO3 = "Н.Д.";
                            }
                        }
                        catch { }

                        customDataPanel1.SetDataToFields(new List<string>() { data.Temperature, data.Humidity, data.AmountClouds, data.Visibility1, data.Visibility10, data.NGO1, data.NGO2, data.NGO3 }, skydexIndex );

                        //Пересчет для высоты

                        var tempPresGPA = 0.0;
                        var tempPresRtSt = 0.0;
                        try
                        {
                            //tempPresGPA = Convert.ToDouble(data.PressureGPa);
                            //try
                            //{
                                tempPresGPA = Convert.ToDouble(data.PressureGPa) * Math.Exp((0.029 * 9.81 * Convert.ToDouble(conf.HEIGHT)) / (8.31 * (Convert.ToDouble(data.Temperature) + 273.15)));
                            //}
                            //catch { }

                            tempPresRtSt = tempPresGPA * 0.750063755419211;
                        }
                        catch
                        {

                        }

                        if (tempPresGPA == 0.0 && tempPresRtSt == 0.0)
                        {
                            customDadDataPanel1.SetDataToFields(new List<string>() { data.PressureRtSt, data.PressureGPa, data.BarTend, data.Trend, "Н.Д.", "Н.Д."});
                        }
                        else
                        {
                            customDadDataPanel1.SetDataToFields(new List<string>() { data.PressureRtSt, data.PressureGPa, data.BarTend, data.Trend, Math.Round(tempPresRtSt, 1).ToString("F1"), Math.Round(tempPresGPA, 1).ToString("F1") });
                        }

                        if (data.ShipSpeed != "Н.Д.") data.ShipSpeed = (Convert.ToString(Math.Round(double.Parse(data.ShipSpeed) * 1.94384449244, 1)));

                        customBottomDataPanel1.SetDataToFields(new List<string>() { data.CourseShip, data.ShipSpeed });

                        customRoundWindPanel1.ValueSpeed = data.Speed_K.Trim();
                        customRoundWindPanel2.ValueSpeed = data.Speed_I.Trim();
                        customRoundWindPanel1.ValueDir = data.Direction_K.Trim();
                        customRoundWindPanel2.ValueDir = data.Direction_I.Trim();
                        customRoundWindPanel1.ValueCurs = "0";
                        customRoundWindPanel2.ValueCurs = data.CourseShip.Trim();

                        customRoundWindPanel1.ValueDir_2mid = data.Direction_2Kmid.Trim();
                        customRoundWindPanel1.ValueDir_2min = data.Direction_2Kmin.Trim();
                        customRoundWindPanel1.ValueDir_2max = data.Direction_2Kmax.Trim();

                        customRoundWindPanel1.ValueDir_10mid = data.Direction_10Kmid.Trim();
                        customRoundWindPanel1.ValueDir_10min = data.Direction_10Kmin.Trim();
                        customRoundWindPanel1.ValueDir_10max = data.Direction_10Kmax.Trim();

                        customRoundWindPanel2.ValueDir_2mid = data.Direction_2Imid.Trim();
                        customRoundWindPanel2.ValueDir_2min = data.Direction_2Imin.Trim();
                        customRoundWindPanel2.ValueDir_2max = data.Direction_2Imax.Trim();

                        customRoundWindPanel2.ValueDir_10mid = data.Direction_10Imid.Trim();
                        customRoundWindPanel2.ValueDir_10min = data.Direction_10Imin.Trim();
                        customRoundWindPanel2.ValueDir_10max = data.Direction_10Imax.Trim();
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

            if ((conf.DSNV1 == "1") && (conf.DSNV2 == "1"))
            {
                if (data.StatusSpeed1 == "/" || data.StatusSpeed1 == "0") status = "0";
                toolTipText += (data.StatusSpeed1 == "/") ? "Отключен основной канал скорости ветра;\r\n" :
                    (data.StatusSpeed1 == "0") ? "Авария по основному каналу скорости ветра;\r\n" : "";
                if (data.StatusDirect1 == "/" || data.StatusDirect1 == "0") status = "0";
                toolTipText += (data.StatusDirect1 == "/") ? "Отключен основной канал направления ветра;\r\n" :
                    (data.StatusDirect1 == "0") ? "Авария по основному каналу направления ветра;\r\n" : "";

                if (data.StatusSpeed2 == "/" || data.StatusSpeed2 == "0") status = "0";
                toolTipText += (data.StatusSpeed2 == "/") ? "Отключен резервный канал скорости ветра;\r\n" :
                    (data.StatusSpeed2 == "0") ? "Авария по резервному каналу скорости ветра;\r\n" : "";
                if (data.StatusDirect2 == "/" || data.StatusDirect2 == "0") status = "0";
                toolTipText += (data.StatusDirect2 == "/") ? "Отключен резервный канал направления ветра;\r\n" :
                    (data.StatusDirect2 == "0") ? "Авария по резервному каналу направления ветра;\r\n" : "";
            }

            if (conf.DSNV3 == "1")
            {
                if (data.StatusSpeedNasal == "/" || data.StatusSpeedNasal == "0") status = "0";
                toolTipText += (data.StatusSpeedNasal == "/") ? "Отключен канал скорости ветра ВПП;\r\n" :
                    (data.StatusSpeedNasal == "0") ? "Авария по каналу скорости ветра ВПП;\r\n" : "";
                if (data.StatusDirectNasal == "/" || data.StatusDirectNasal == "0") status = "0";
                toolTipText += (data.StatusDirectNasal == "/") ? "Отключен канал направления ветра ВПП;\r\n" :
                    (data.StatusDirectNasal == "0") ? "Авария по каналу направления ветра ВПП;\r\n" : "";
            }

            if ((conf.DSNV1 == "1") && (conf.DSNV2 == "0") && (conf.DSNV3 == "0"))
            {
                if (data.StatusSpeed1 == "/" || data.StatusSpeed1 == "0") status = "0";
                toolTipText += (data.StatusSpeed1 == "/") ? "Отключен канал скорости ветра;\r\n" :
                    (data.StatusSpeed1 == "0") ? "Авария по каналу скорости ветра;\r\n" : "";
                if (data.StatusDirect1 == "/" || data.StatusDirect1 == "0") status = "0";
                toolTipText += (data.StatusDirect1 == "/") ? "Отключен канал направления ветра;\r\n" :
                    (data.StatusDirect1 == "0") ? "Авария по каналу направления ветра;\r\n" : "";
            }

            if (conf.DTVV == "1")
            {
                if (data.StatusTemp1 == "/" || data.StatusTemp1 == "0") status = "0";
                toolTipText += (data.StatusTemp1 == "/") ? "Отключен канал температуры ;\r\n" :
                    (data.StatusTemp1 == "0") ? "Авария по каналу температуры;\r\n" : "";
                if (data.StatusHum1 == "/" || data.StatusHum1 == "0") status = "0";
                toolTipText += (data.StatusHum1 == "/") ? "Отключен канал влажности;\r\n" :
                    (data.StatusHum1 == "0") ? "Авария по каналу влажности;\r\n" : "";
            }

            if (conf.DAD1 == "1" && conf.DAD2 == "1")
            {
                if (data.StatusPressure1 == "/" || data.StatusPressure1 == "0") status = "0";
                toolTipText += (data.StatusPressure1 == "/") ? "Отключен основной канал атмосферного\r\nдавления;\r\n" :
                    (data.StatusPressure1 == "0") ? "Авария по основному каналу атмосферного\r\nдавления;\r\n" : "";

                if (data.StatusPressure2 == "/" || data.StatusPressure2 == "0") status = "0";
                toolTipText += (data.StatusPressure2 == "/") ? "Отключен резервный канал атмосферного\r\nдавления;\r\n" :
                    (data.StatusPressure2 == "0") ? "Авария по резервному каналу атмосферного\r\nдавления;\r\n" : "";
            }

            if (conf.DAD1 == "1" && conf.DAD2 == "0")
            {
                if (data.StatusPressure1 == "/" || data.StatusPressure1 == "0") status = "0";
                toolTipText += (data.StatusPressure1 == "/") ? "Отключен канал атмосферного давления;\r\n" :
                    (data.StatusPressure1 == "0") ? "Авария по каналу атмосферного давления;\r\n" : "";
            }

            if (conf.DVGO == "1")
            {
                if (data.StatusDVGO == "/" || data.StatusDVGO == "A" || data.StatusDVGO == "W") status = "0";
                toolTipText += (data.StatusDVGO == "/") ? "Отключен датчик верхней границы облаков;\r\n" :
                    (data.StatusDVGO == "A") ? "Авария по датчику верхней границы облаков;\r\n" :
                    (data.StatusDVGO == "W") ? "Тревога по датчику верхней границы облаков;\r\n" : "";
            }

            if (conf.DMDV == "1")
            {
                if (data.StatusDMDV == "/" || data.StatusDMDV == "1" || data.StatusDMDV == "2" || data.StatusDMDV == "3" || data.StatusDMDV == "4") status = "0";
                toolTipText += (data.StatusDMDV == "/") ? "Отключен датчик метеорологической\r\nдальности видимости;\r\n" :
                    (data.StatusDMDV == "1") ? "Ошибка оборудования по датчику\r\nметеорологической дальности видимости;\r\n" :
                    (data.StatusDMDV == "2") ? "Предупреждение по оборудованию датчика\r\nметеорологической дальности видимости;\r\n" :
                    (data.StatusDMDV == "3") ? "Тревога по обратному рассеянию датчика\r\nметеорологической дальности видимости;\r\n" :
                    (data.StatusDMDV == "4") ? "Предупреждение по обратному рассеянию датчика\r\nметеорологической дальности видимости;\r\n" : "";
            }


            if (data.ShipSpeed == "Н.Д.") status = "0";
            toolTipText += (data.ShipSpeed == "Н.Д.") ? "Нет скорости корабля;\r\n" : "";

            if (data.CourseShip == "Н.Д.") status = "0";
            toolTipText += (data.CourseShip == "Н.Д.") ? "Нет курса корабля;\r\n" : "";

            return new List<string> { status, toolTipText };
        }


    }
}
