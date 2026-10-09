using System;

namespace Capriz_WPF.Common
{
    /// <summary>
    /// Преобразование числовых/символьных кодов статусов в текст.
    /// </summary>
    public static class StatusConverter
    {
        /// <summary>
        /// Датчики с кодами 0/1: 0 = ошибка, 1 = норма.
        /// </summary>
        public static string Status01ToText(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "Н.Д.";
            switch (value.Trim())
            {
                case "0": return "Ошибка";
                case "1": return "Норма";
                case "/": return "Отключён";
                default: return value.Trim();
            }
        }

        /// <summary>
        /// SKYDEX: 0 = норма, A = авария, W = тревога.
        /// </summary>
        public static string SkydexToText(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "Н.Д.";
            switch (value.Trim())
            {
                case "0": return "Норма";
                case "A": return "Авария";
                case "W": return "Тревога";
                case "/": return "Отключён";
                default: return value.Trim();
            }
        }

        /// <summary>
        /// ДМДВ: 0 = норма, 1..3 — различные ошибки.
        /// </summary>
        public static string DmdvToText(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "Н.Д.";
            switch (value.Trim())
            {
                case "0": return "Норма";
                case "1": return "Ошибка оборудования";
                case "2": return "Предупреждение по оборудованию";
                case "3": return "Тревога по обратному рассеянию";
                case "/": return "Отключён";
                default: return value.Trim();
            }
        }

        /// <summary>
        /// Количество слоёв облаков: 0..5.
        /// </summary>
        public static string AmountCloudsToText(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "Н.Д.";
            if (!int.TryParse(value.Trim(), out int n)) return value.Trim();
            if (n < 0 || n > 5) return value.Trim();
            return n.ToString();
        }

        /// <summary>
        /// Сокращённые названия для заголовков колонок.
        /// </summary>
        public static string ShortHeader(string fullName)
        {
            switch (fullName)
            {
                case "StatusTemp1": return "Темп. ДМП-1 №1";
                case "StatusTemp2": return "Темп. ДМП-1 №2";
                case "StatusHum1": return "Влажн. ДМП-1 №1";
                case "StatusHum2": return "Влажн. ДМП-1 №2";
                case "StatusWind1": return "Ветер ДМП-1 №1";
                case "StatusWind2": return "Ветер ДМП-1 №2";
                case "StatusWindWMT": return "Ветер WMT-702";
                case "StatusPressure1": return "Давл. ДМП-1 №1";
                case "StatusPressure2": return "Давл. ДМП-1 №2";
                case "StatusSKYDEX": return "SKYDEX-15";
                case "AmountClouds": return "Слоёв облаков";
                case "StatusDMDV": return "ДМДВ";
                default: return fullName;
            }
        }
    }
}