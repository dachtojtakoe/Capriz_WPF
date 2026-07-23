using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Capriz_WPF.Common
{
    public static class Settings
    {
        public static void CreateFolder()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + Path.DirectorySeparatorChar + @"Capriz";
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            Environment.CurrentDirectory = dir;
        }

        public static void CCulture()
        {
            CultureInfo _culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            CultureInfo _uiculture = (CultureInfo)Thread.CurrentThread.CurrentUICulture.Clone();

            _culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            _uiculture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;

            _culture.NumberFormat.NumberDecimalSeparator = ".";
            _uiculture.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = _culture;
            Thread.CurrentThread.CurrentUICulture = _uiculture;
        }
    }
}
