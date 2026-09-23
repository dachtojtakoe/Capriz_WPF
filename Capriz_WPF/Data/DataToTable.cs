using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Capriz_WPF.Data
{
    static class DataToTable
    {
        // Единый список — используется и в парсере, и в схеме БД
        public static readonly string[] Columns = new[]
        {
            "Temperature","Humidity","PressureGPa","PressureRtSt","BarTend","Trend",
            "Speed_K","Direction_K","Speed_I","Direction_I",
            "Direction_2Kmid","Direction_2Kmin","Direction_2Kmax",
            "Direction_10Kmid","Direction_10Kmin","Direction_10Kmax",
            "Direction_2Imid","Direction_2Imin","Direction_2Imax",
            "Direction_10Imid","Direction_10Imin","Direction_10Imax",
            "Speed_2Kmid","Speed_2Kmin","Speed_2Kmax",
            "Speed_10Kmid","Speed_10Kmin","Speed_10Kmax",
            "Speed_2Imid","Speed_2Imin","Speed_2Imax",
            "Speed_10Imid","Speed_10Imin","Speed_10Imax",

            "Speed_K2","Direction_K2","Speed_I2","Direction_I2",
            "Direction_2Kmid_2","Direction_2Kmin_2","Direction_2Kmax_2",
            "Direction_10Kmid_2","Direction_10Kmin_2","Direction_10Kmax_2",
            "Direction_2Imid_2","Direction_2Imin_2","Direction_2Imax_2",
            "Direction_10Imid_2","Direction_10Imin_2","Direction_10Imax_2",
            "Speed_2Kmid_2","Speed_2Kmin_2","Speed_2Kmax_2",
            "Speed_10Kmid_2","Speed_10Kmin_2","Speed_10Kmax_2",
            "Speed_2Imid_2","Speed_2Imin_2","Speed_2Imax_2",
            "Speed_10Imid_2","Speed_10Imin_2","Speed_10Imax_2",

            "Speed_Kw","Direction_Kw","Speed_Iw","Direction_Iw",
            "Direction_2Kmid_w","Direction_2Kmin_w","Direction_2Kmax_w",
            "Direction_10Kmid_w","Direction_10Kmin_w","Direction_10Kmax_w",
            "Direction_2Imid_w","Direction_2Imin_w","Direction_2Imax_w",
            "Direction_10Imid_w","Direction_10Imin_w","Direction_10Imax_w",
            "Speed_2Kmid_w","Speed_2Kmin_w","Speed_2Kmax_w",
            "Speed_10Kmid_w","Speed_10Kmin_w","Speed_10Kmax_w",
            "Speed_2Imid_w","Speed_2Imin_w","Speed_2Imax_w",
            "Speed_10Imid_w","Speed_10Imin_w","Speed_10Imax_w",

            "Visibility10","Visibility1","AmountPrecipitation",
            "ShipSpeed","CourseShip","NGO1","NGO2","NGO3",
            "StatusTemp1","StatusTemp2","StatusHum1","StatusHum2",
            "StatusWind1","StatusWind2","StatusWindWMT",
            "StatusPressure1","StatusPressure2","StatusSKYDEX",
            "AmountClouds","StatusDMDV",
            "LatDeg","LatMin","LatSec","LatNS",
            "LonDeg","LonMin","LonSec","LonEW",
            "Hm0","Hmax"
        };

        public static DataTable CreateDt()
        {
            var dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            foreach (var c in Columns)
                dt.Columns.Add(c, typeof(string));
            return dt;
        }

        public static DataTable CreateDtGraphic()
        {
            var dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(string));
            dt.Columns.Add("Temperature", typeof(string));
            dt.Columns.Add("Humidity", typeof(string));
            dt.Columns.Add("PressureGPa", typeof(string));
            dt.Columns.Add("Speed_I", typeof(string));
            dt.Columns.Add("Visibility10", typeof(string));
            return dt;
        }

        public static DataTable CopyDt(DataTable from)
        {
            var dt = CreateDtGraphic();
            foreach (DataRow r in from.Rows)
            {
                var row = dt.NewRow();
                row["DateTime"] = Convert.ToDateTime(r["DateTime"]).ToString("dd.MM.yyyy HH:mm:ss");
                row["Temperature"] = r["Temperature"];
                row["Humidity"] = r["Humidity"];
                row["PressureGPa"] = r["PressureGPa"];
                row["Speed_I"] = r["Speed_I"];
                row["Visibility10"] = r["Visibility10"];
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static DataTable CopyDtChart(DataTable from, string name)
        {
            var dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add(name, typeof(double));
            foreach (DataRow r in from.Rows)
            {
                var row = dt.NewRow();
                row["Date"] = r["Date"];
                row["Time"] = r["Time"];
                row[name] = r[name].ToString() == "Н.Д." ? -999 : r[name];
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static DataTable CopyDtChart2(DataTable from, string name)
        {
            var dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add(name, typeof(string));
            foreach (DataRow r in from.Rows)
            {
                var row = dt.NewRow();
                row["Date"] = r["Date"];
                row["Time"] = r["Time"];
                row[name] = r[name];
                dt.Rows.Add(row);
            }
            return dt;
        }

        // Парсер: 1-в-1 совпадает с порядком свойств в Data.Data
        public static List<Data> DataToList(string allData)
        {
            var list = new List<Data>();
            try
            {
                var cols = allData.Split('\t', '\x02', '\x03', '*');
                if (cols.Length < Columns.Length + 2) return null;

                var d = new Data();
                var props = typeof(Data).GetProperties()
                    .Where(p => p.Name != "Date" && p.Name != "Time")
                    .ToArray();

                for (int i = 0; i < Columns.Length; i++)
                {
                    string raw = cols[i + 1];
                    string val;
                    if (Columns[i].StartsWith("Status") || Columns[i] == "AmountClouds")
                        val = raw.Contains("/") ? "/" : raw;   // статусы: / сохраняем
                    else
                        val = raw.Contains("/") ? "Н.Д." : raw;

                    var prop = props.FirstOrDefault(p => p.Name == Columns[i]);
                    if (prop != null) prop.SetValue(d, val);
                }
                list.Add(d);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DataToList",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return list;
        }
    }
}