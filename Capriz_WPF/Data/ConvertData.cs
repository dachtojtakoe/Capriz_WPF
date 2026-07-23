using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capriz_WPF.Data
{
    public static class ConvertData
    {
        public static string GetMessage(byte[] str)
        {
            var result = string.Empty;
            foreach (byte item in str)
                result += Convert.ToChar(item);
            return result;
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
            {"DateTime","Дата и время"},
            {"Температура воздуха, °C", "Temperatura, °C"},
            {"Влажность воздуха, %","Vlaznost vozduha, %"},
            {"Атмосферное давление, гПа","Atmosphernoe davlenie, gPa"},
            {"Скорость истинного ветра, м/с", "Skorost istonnogo vetra, m/c"},
            {"Метеорологическая дальность видимости за 10 мин, м", "Meteorologicheskaya dalnost vidimosti za 10 min, m" }
        };

        public static string GetNameColumn(string str)
        {
            return nameColumns[str];
        }

        public static string GetNameColumnBack(string str)
        {
            return nameColumnsBack[str];
        }
    }
}
