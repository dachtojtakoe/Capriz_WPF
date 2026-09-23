using System;

namespace Capriz_WPF.Data
{
    public class Data
    {
        // служебные
        public string Date { get; set; }
        public string Time { get; set; }

        // 1-6: погода
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string PressureGPa { get; set; }
        public string PressureRtSt { get; set; }
        public string BarTend { get; set; }
        public string Trend { get; set; }

        // 7-10: ДМП-1 №1 мгновенные
        public string Speed_K { get; set; }
        public string Direction_K { get; set; }
        public string Speed_I { get; set; }
        public string Direction_I { get; set; }

        // 11-22: ДМП-1 №1 направления
        public string Direction_2Kmid { get; set; }
        public string Direction_2Kmin { get; set; }
        public string Direction_2Kmax { get; set; }
        public string Direction_10Kmid { get; set; }
        public string Direction_10Kmin { get; set; }
        public string Direction_10Kmax { get; set; }
        public string Direction_2Imid { get; set; }
        public string Direction_2Imin { get; set; }
        public string Direction_2Imax { get; set; }
        public string Direction_10Imid { get; set; }
        public string Direction_10Imin { get; set; }
        public string Direction_10Imax { get; set; }

        // 23-34: ДМП-1 №1 скорости
        public string Speed_2Kmid { get; set; }
        public string Speed_2Kmin { get; set; }
        public string Speed_2Kmax { get; set; }
        public string Speed_10Kmid { get; set; }
        public string Speed_10Kmin { get; set; }
        public string Speed_10Kmax { get; set; }
        public string Speed_2Imid { get; set; }
        public string Speed_2Imin { get; set; }
        public string Speed_2Imax { get; set; }
        public string Speed_10Imid { get; set; }
        public string Speed_10Imin { get; set; }
        public string Speed_10Imax { get; set; }

        // 35-62: ДМП-1 №2
        public string Speed_K2 { get; set; }
        public string Direction_K2 { get; set; }
        public string Speed_I2 { get; set; }
        public string Direction_I2 { get; set; }
        public string Direction_2Kmid_2 { get; set; }
        public string Direction_2Kmin_2 { get; set; }
        public string Direction_2Kmax_2 { get; set; }
        public string Direction_10Kmid_2 { get; set; }
        public string Direction_10Kmin_2 { get; set; }
        public string Direction_10Kmax_2 { get; set; }
        public string Direction_2Imid_2 { get; set; }
        public string Direction_2Imin_2 { get; set; }
        public string Direction_2Imax_2 { get; set; }
        public string Direction_10Imid_2 { get; set; }
        public string Direction_10Imin_2 { get; set; }
        public string Direction_10Imax_2 { get; set; }
        public string Speed_2Kmid_2 { get; set; }
        public string Speed_2Kmin_2 { get; set; }
        public string Speed_2Kmax_2 { get; set; }
        public string Speed_10Kmid_2 { get; set; }
        public string Speed_10Kmin_2 { get; set; }
        public string Speed_10Kmax_2 { get; set; }
        public string Speed_2Imid_2 { get; set; }
        public string Speed_2Imin_2 { get; set; }
        public string Speed_2Imax_2 { get; set; }
        public string Speed_10Imid_2 { get; set; }
        public string Speed_10Imin_2 { get; set; }
        public string Speed_10Imax_2 { get; set; }

        // 63-90: WMT-702
        public string Speed_Kw { get; set; }
        public string Direction_Kw { get; set; }
        public string Speed_Iw { get; set; }
        public string Direction_Iw { get; set; }
        public string Direction_2Kmid_w { get; set; }
        public string Direction_2Kmin_w { get; set; }
        public string Direction_2Kmax_w { get; set; }
        public string Direction_10Kmid_w { get; set; }
        public string Direction_10Kmin_w { get; set; }
        public string Direction_10Kmax_w { get; set; }
        public string Direction_2Imid_w { get; set; }
        public string Direction_2Imin_w { get; set; }
        public string Direction_2Imax_w { get; set; }
        public string Direction_10Imid_w { get; set; }
        public string Direction_10Imin_w { get; set; }
        public string Direction_10Imax_w { get; set; }
        public string Speed_2Kmid_w { get; set; }
        public string Speed_2Kmin_w { get; set; }
        public string Speed_2Kmax_w { get; set; }
        public string Speed_10Kmid_w { get; set; }
        public string Speed_10Kmin_w { get; set; }
        public string Speed_10Kmax_w { get; set; }
        public string Speed_2Imid_w { get; set; }
        public string Speed_2Imin_w { get; set; }
        public string Speed_2Imax_w { get; set; }
        public string Speed_10Imid_w { get; set; }
        public string Speed_10Imin_w { get; set; }
        public string Speed_10Imax_w { get; set; }

        // 91-98: видимость / осадки / судно / НГО
        public string Visibility10 { get; set; }
        public string Visibility1 { get; set; }
        public string AmountPrecipitation { get; set; }
        public string ShipSpeed { get; set; }
        public string CourseShip { get; set; }
        public string NGO1 { get; set; }
        public string NGO2 { get; set; }
        public string NGO3 { get; set; }

        // 99-110: статусы
        public string StatusTemp1 { get; set; }
        public string StatusTemp2 { get; set; }
        public string StatusHum1 { get; set; }
        public string StatusHum2 { get; set; }
        public string StatusWind1 { get; set; }
        public string StatusWind2 { get; set; }
        public string StatusWindWMT { get; set; }
        public string StatusPressure1 { get; set; }
        public string StatusPressure2 { get; set; }
        public string StatusSKYDEX { get; set; }
        public string AmountClouds { get; set; }
        public string StatusDMDV { get; set; }

        // 111-118: координаты
        public string LatDeg { get; set; }
        public string LatMin { get; set; }
        public string LatSec { get; set; }
        public string LatNS { get; set; }
        public string LonDeg { get; set; }
        public string LonMin { get; set; }
        public string LonSec { get; set; }
        public string LonEW { get; set; }

        // 119-120: волнение
        public string Hm0 { get; set; }
        public string Hmax { get; set; }
    }
}