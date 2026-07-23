using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static DataLite CalculateTrueWind(DataLite dt)
        {
            double shipSpeed = Convert.ToDouble(dt.ShipSpeed.Replace('.', ','));
            double shipDirection = Convert.ToDouble(dt.ShipCourse.Replace('.', ','));

            double Vk = Convert.ToDouble(dt.Speed_K.Replace('.', ',')); // м/с - постоянная составляющая значения скорости кажущегося ветра
            //Vk *= 1.94; // 9.72 скорость в узлах

            double dk = Convert.ToDouble(dt.Direction_K.Replace('.', ',')); // град - постоянная составляющая направления ветра

            Stopwatch sw = Stopwatch.StartNew();

            double cursWindDirection = shipDirection - 180;
            if (cursWindDirection < 0)
                cursWindDirection += 360;

            double actualWindSpeed = Vk;
            double actualWindDirection = dk;

            double wspd = Vk;
            double wdir = dk;

            //// Преобразование градусов в радианы
            double shipDirectionRad = cursWindDirection * Math.PI / 180;
            double apparentWindDirectionRad = wdir * Math.PI / 180;

            //Разложение векторов на компоненты
            double shipX = shipSpeed * Math.Cos(shipDirectionRad);
            double shipY = shipSpeed * Math.Sin(shipDirectionRad);

            double apparentWindX = wspd * Math.Cos(apparentWindDirectionRad);
            double apparentWindY = wspd * Math.Sin(apparentWindDirectionRad);

            // Вычисление компонент истинного ветра
            double trueWindX = apparentWindX - shipX;
            double trueWindY = apparentWindY - shipY;

            // Вычисление скорости истинного ветра
            double trueWindSpeed = Math.Sqrt(trueWindX * trueWindX + trueWindY * trueWindY);

            // Вычисление направления истинного ветра (в радианах)
            double trueWindDirectionRad = Math.Atan2(trueWindY, trueWindX);

            // Преобразование направления в градусы
            double trueWindDirection = trueWindDirectionRad * 180 / Math.PI;

            //Корректировка направления(0 - 360 градусов)
            if (trueWindDirection < 0)
                trueWindDirection += 360;

            Console.WriteLine($"Скорость кажущегося ветра, за 2 мин: {wspd} узлов");
            Console.WriteLine($"Направление кажущегося ветра, за 2 мин: {wdir} градусов");
            Console.WriteLine($"Скорость судна ветра: {shipSpeed} узлов");
            Console.WriteLine($"Направление судна: {shipDirection} градусов");
            Console.WriteLine($"Направление курсового ветра: {cursWindDirection} градусов");

            Console.WriteLine($"Разница направлений: {Math.Abs(cursWindDirection - wdir)} градусов");

            Console.WriteLine($"Скорость истинного ветра: {trueWindSpeed} узлов");
            Console.WriteLine($"Направление истинного ветра: {trueWindDirection} градусов");
            Console.WriteLine($"");

            dt.Speed_I = Math.Round(trueWindSpeed, 2).ToString();
            dt.Direction_I = Math.Round(trueWindDirection).ToString();
            dt.SpeedIList = trueWindSpeed;
            dt.DirectionIList = trueWindDirection;

            return dt;
        }
    }
}
