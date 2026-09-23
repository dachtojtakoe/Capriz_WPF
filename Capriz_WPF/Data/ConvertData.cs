using System;
using System.Collections.Generic;
using System.Text;

namespace Capriz_WPF.Data
{
    public static class ConvertData
    {
        private const int FrameSize = 404;

        private static string F(byte[] b, int pos, int len)
        {
            int start = pos - 1;
            if (start < 0 || start + len > b.Length) return "Н.Д.";
            string s = Encoding.ASCII.GetString(b, start, len).Trim();
            return string.IsNullOrEmpty(s) ? "Н.Д." : s;
        }

        private static string S(byte[] b, int pos, int len) // статус (сохраняем "/")
        {
            int start = pos - 1;
            if (start < 0 || start + len > b.Length) return "/";
            string s = Encoding.ASCII.GetString(b, start, len).Trim();
            return string.IsNullOrEmpty(s) ? "/" : s;
        }

        private static bool CheckCrc(byte[] b)
        {
            if (b == null || b.Length < FrameSize) return false;
            if (b[398] != 0x03 || b[399] != 0x2A) return false;
            byte cs = 0;
            for (int i = 0; i < 399; i++) cs ^= b[i];
            string hex = cs.ToString("X2");
            return b[400] == (byte)hex[0] && b[401] == (byte)hex[1];
        }

        public static string GetMessage(byte[] b)
        {
            if (!CheckCrc(b)) return "";

            var sb = new StringBuilder(FrameSize * 2);
            sb.Append("$ALB").Append('\x02');

            // 1-6 погода
            sb.Append(F(b, 6, 5)).Append('\t');
            sb.Append(F(b, 11, 3)).Append('\t');
            sb.Append(F(b, 14, 6)).Append('\t');
            sb.Append(F(b, 20, 5)).Append('\t');
            sb.Append(F(b, 25, 5)).Append('\t');
            sb.Append(F(b, 30, 1)).Append('\t');

            // 7-10 мгновенные №1
            sb.Append(F(b, 31, 4)).Append('\t');
            sb.Append(F(b, 35, 3)).Append('\t');
            sb.Append(F(b, 38, 4)).Append('\t');
            sb.Append(F(b, 42, 3)).Append('\t');

            // 11-22 направления №1
            for (int i = 0; i < 6; i++) sb.Append(F(b, 45 + i * 3, 3)).Append('\t'); // 2/10 K
            for (int i = 0; i < 6; i++) sb.Append(F(b, 63 + i * 3, 3)).Append('\t'); // 2/10 I
            // 23-34 скорости №1
            for (int i = 0; i < 6; i++) sb.Append(F(b, 81 + i * 4, 4)).Append('\t'); // 2/10 K
            for (int i = 0; i < 6; i++) sb.Append(F(b, 105 + i * 4, 4)).Append('\t'); // 2/10 I

            // 35-62 №2
            sb.Append(F(b, 129, 4)).Append('\t');
            sb.Append(F(b, 133, 3)).Append('\t');
            sb.Append(F(b, 136, 4)).Append('\t');
            sb.Append(F(b, 140, 3)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 143 + i * 3, 3)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 161 + i * 3, 3)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 179 + i * 4, 4)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 203 + i * 4, 4)).Append('\t');

            // 63-90 WMT-702
            sb.Append(F(b, 227, 4)).Append('\t');
            sb.Append(F(b, 231, 3)).Append('\t');
            sb.Append(F(b, 234, 4)).Append('\t');
            sb.Append(F(b, 238, 3)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 241 + i * 3, 3)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 259 + i * 3, 3)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 277 + i * 4, 4)).Append('\t');
            for (int i = 0; i < 6; i++) sb.Append(F(b, 301 + i * 4, 4)).Append('\t');

            // 91-98 видимость / осадки / судно / НГО
            sb.Append(F(b, 325, 5)).Append('\t');
            sb.Append(F(b, 330, 5)).Append('\t');
            sb.Append(F(b, 335, 6)).Append('\t');
            sb.Append(F(b, 341, 5)).Append('\t');
            sb.Append(F(b, 346, 3)).Append('\t');
            sb.Append(F(b, 349, 4)).Append('\t');
            sb.Append(F(b, 353, 4)).Append('\t');
            sb.Append(F(b, 357, 4)).Append('\t');

            // 99-110 статусы (сохраняем "/"!)
            sb.Append(S(b, 361, 1)).Append('\t'); // StatusTemp1
            sb.Append(S(b, 362, 1)).Append('\t'); // StatusTemp2
            sb.Append(S(b, 363, 1)).Append('\t'); // StatusHum1
            sb.Append(S(b, 364, 1)).Append('\t'); // StatusHum2
            sb.Append(S(b, 365, 1)).Append('\t'); // StatusWind1
            sb.Append(S(b, 366, 1)).Append('\t'); // StatusWind2
            sb.Append(S(b, 367, 1)).Append('\t'); // StatusWindWMT
            sb.Append(S(b, 368, 1)).Append('\t'); // StatusPressure1
            sb.Append(S(b, 369, 1)).Append('\t'); // StatusPressure2
            sb.Append(S(b, 370, 1)).Append('\t'); // StatusSKYDEX
            sb.Append(S(b, 371, 1)).Append('\t'); // AmountClouds
            sb.Append(S(b, 372, 1)).Append('\t'); // StatusDMDV

            // 111-118 координаты
            sb.Append(F(b, 373, 2)).Append('\t');
            sb.Append(F(b, 375, 2)).Append('\t');
            sb.Append(F(b, 377, 2)).Append('\t');
            sb.Append(F(b, 379, 1)).Append('\t');
            sb.Append(F(b, 380, 2)).Append('\t');
            sb.Append(F(b, 382, 2)).Append('\t');
            sb.Append(F(b, 384, 2)).Append('\t');
            sb.Append(F(b, 386, 1)).Append('\t');

            // 119-120 волнение
            sb.Append(F(b, 387, 6)).Append('\t');
            sb.Append(F(b, 393, 6));

            sb.Append('\x03').Append('*')
              .Append((char)b[400]).Append((char)b[401])
              .Append('\r').Append('\n');

            return sb.ToString();
        }

        private static Dictionary<string, string> nameColumns = new Dictionary<string, string>()
        {
            {"DateTime","Дата и время"},
            {"Temperature","Температура воздуха, °C"},
            {"Humidity","Влажность воздуха, %"},
            {"PressureGPa","Атмосферное давление, гПа"},
            {"Speed_I","Скорость истинного ветра, м/с"},
            {"Visibility10","Метеорологическая дальность видимости за 10 мин, м" }
        };

        private static Dictionary<string, string> nameColumnsBack = new Dictionary<string, string>()
        {   
            { "DateTime", "Дата и время" },
            {"Температура воздуха, °C", "Temperatura, °C"},
            {"Влажность воздуха, %","Vlaznost vozduha, %"},
            { "Атмосферное давление, гПа","Atmosphernoe davlenie, gPa"},
            { "Скорость истинного ветра, м/с", "Skorost istonnogo vetra, m/c"},
            { "Метеорологическая дальность видимости за 10 мин, м", "Meteorologicheskaya dalnost vidimosti za 10 min, m" }
        };

        public static string GetNameColumn(string str)
        {
            return nameColumns[str];
        }

        //public static string GetNameColumnBack(string str)
        //{
        //    return nameColumnsBack[str];
        //}
    }
}