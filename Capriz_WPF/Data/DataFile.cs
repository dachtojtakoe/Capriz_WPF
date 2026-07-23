using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;

namespace Capriz_WPF.Data
{
    public static class DataFile
    {
        public static string GetFileName()
        {
            string fileName = "";
            using (OpenFileDialog openFile = new OpenFileDialog() { Filter = "LOG FILES (*.txt;*.log)|*.txt;*.log", ValidateNames = true })
            {
                openFile.InitialDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
                if (openFile.ShowDialog() == DialogResult.OK)
                    fileName = openFile.FileName;
                return fileName;
            }
        }


        static string WriteFile(string param)
        {
            //Записываем в файл
            string path = LogsPath();
            TextWriter writer = new StreamWriter(path);
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            try
            {
                writer.Write(param);
                writer.Flush();
                writer.Close();
            }
            catch { }
            return path;
        }


        static public string CurrentDirectory() => Directory.GetCurrentDirectory();

        static public string LogsPath()
        {
            string logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            string filePath = Path.Combine(logsDirectory, $"Log-{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")}.txt");
            return filePath;
        }

        static public string SettingsPath() => Path.Combine(Directory.GetCurrentDirectory() +
            Path.DirectorySeparatorChar,
            $"Settings.ini");

        static public string ChartPath(string ChartName, string format)
        {
            string chartsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Charts");

            if (!Directory.Exists(Path.Combine(chartsDirectory, format)))
            {
                Directory.CreateDirectory(Path.Combine(chartsDirectory, format));
            }

            string filePath = Path.Combine(chartsDirectory, format, $"{ChartName}-{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")}.{format}");
            return filePath;
        }
        //static public string ChartPath(string ChartName) => Path.Combine(Directory.GetCurrentDirectory() +
        //    Path.DirectorySeparatorChar + "Charts" + Path.DirectorySeparatorChar,
        //    $"{ChartName}-{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")}.pdf");

        public static DataTable DataToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);//Get all the properties

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);//Setting column names as Property names
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);//inserting property values to datatable rows
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static void WriteDataToFile(string filename, string param)
        {
            //Запись                    
            using (StreamWriter Writer = new StreamWriter(filename, true))
            {
                Writer.Write(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss ") + param);
                Writer.Close();
            }
        }

        public static void WriteDataToFileALot(string filename, string param, int day)
        {
            //Запись                    
            using (StreamWriter Writer = new StreamWriter(filename, true))
            {
                DateTime currentTime = new DateTime(2024, 8, 1);
                currentTime = currentTime.AddDays(day);
                for (int i = 0; i < 8640; i++)
                {
                    Writer.WriteLine(currentTime.ToString("dd.MM.yyyy HH:mm:ss ") + param);
                    currentTime = currentTime.AddSeconds(10);
                }
                Writer.Close();
            }
        }

        public static string ReadDataFromFile(string filename)
        {
            string allData = "";
            //Запись                    
            using (FileStream FileReader = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                allData = new StreamReader(FileReader, Encoding.Default).ReadToEnd();
                FileReader.Close();
                //DataDelegates.EventHandlerStr(allData);
            }
            return allData;
        }

        public static List<Data> DataToList(string allData)
        {
            List<Data> data = new List<Data>();
            string[] lines = allData.Split('\n');
            try
            {
                foreach (var line in lines)
                {
                    var columns = line.Split('\t', '\x02', '\x03', '*');

                    if (columns.GetLength(0) < 81)
                        continue;

                    var dateTime = columns[0].Split(' ');

                    data.Add(new Data
                    {
                        Date = dateTime[0],
                        Time = dateTime[1],
                        Temperature = columns[1].Contains("/") ? "Н.Д." : columns[1],
                        Humidity = columns[2].Contains("/") ? "Н.Д." : columns[2],
                        PressureGPa = columns[3].Contains("/") ? "Н.Д." : columns[3],
                        PressureRtSt = columns[4].Contains("/") ? "Н.Д." : columns[4],
                        BarTend = columns[5].Contains("/") ? "Н.Д." : columns[5],
                        Trend = columns[6].Contains("/") ? "Н.Д." : columns[6],
                        Speed_K = columns[7].Contains("/") ? "Н.Д." : columns[7],
                        Direction_K = columns[8].Contains("/") ? "Н.Д." : columns[8],
                        Speed_I = columns[9].Contains("/") ? "Н.Д." : columns[9],
                        Direction_I = columns[10].Contains("/") ? "Н.Д." : columns[10],
                        Direction_2Kmid = columns[11].Contains("/") ? "Н.Д." : columns[11],
                        Direction_2Kmin = columns[12].Contains("/") ? "Н.Д." : columns[12],
                        Direction_2Kmax = columns[13].Contains("/") ? "Н.Д." : columns[13],
                        Direction_10Kmid = columns[14].Contains("/") ? "Н.Д." : columns[14],
                        Direction_10Kmin = columns[15].Contains("/") ? "Н.Д." : columns[15],
                        Direction_10Kmax = columns[16].Contains("/") ? "Н.Д." : columns[16],
                        Direction_2Imid = columns[17].Contains("/") ? "Н.Д." : columns[17],
                        Direction_2Imin = columns[18].Contains("/") ? "Н.Д." : columns[18],
                        Direction_2Imax = columns[19].Contains("/") ? "Н.Д." : columns[19],
                        Direction_10Imid = columns[20].Contains("/") ? "Н.Д." : columns[20],
                        Direction_10Imin = columns[21].Contains("/") ? "Н.Д." : columns[21],
                        Direction_10Imax = columns[22].Contains("/") ? "Н.Д." : columns[22],
                        Speed_2Kmid = columns[23].Contains("/") ? "Н.Д." : columns[23],
                        Speed_2Kmin = columns[24].Contains("/") ? "Н.Д." : columns[24],
                        Speed_2Kmax = columns[25].Contains("/") ? "Н.Д." : columns[25],
                        Speed_10Kmid = columns[26].Contains("/") ? "Н.Д." : columns[26],
                        Speed_10Kmin = columns[27].Contains("/") ? "Н.Д." : columns[27],
                        Speed_10Kmax = columns[28].Contains("/") ? "Н.Д." : columns[28],
                        Speed_2Imid = columns[29].Contains("/") ? "Н.Д." : columns[29],
                        Speed_2Imin = columns[30].Contains("/") ? "Н.Д." : columns[30],
                        Speed_2Imax = columns[31].Contains("/") ? "Н.Д." : columns[31],
                        Speed_10Imid = columns[32].Contains("/") ? "Н.Д." : columns[32],
                        Speed_10Imin = columns[33].Contains("/") ? "Н.Д." : columns[33],
                        Speed_10Imax = columns[34].Contains("/") ? "Н.Д." : columns[34],
                        Visibility10 = columns[35].Contains("/") ? "Н.Д." : columns[35],
                        Visibility1 = columns[36].Contains("/") ? "Н.Д." : columns[36],
                        AmountPrecipitation = columns[37].Contains("/") ? "Н.Д." : columns[37],
                        ShipSpeed = columns[38].Contains("/") ? "Н.Д." : columns[38],
                        CourseShip = columns[39].Contains("/") ? "Н.Д." : columns[39],
                        NGO1 = columns[40].Contains("/") ? "Н.Д." : columns[40],
                        NGO2 = columns[41].Contains("/") ? "Н.Д." : columns[41],
                        NGO3 = columns[42].Contains("/") ? "Н.Д." : columns[42],
                        StatusTemp1 = columns[43].Contains("/") ? "/" : columns[43],
                        StatusPressure2 = columns[44].Contains("/") ? "/" : columns[44],
                        //StatusTemp2 = columns[44].Contains("/") ? "/" : columns[44],
                        StatusHum1 = columns[45].Contains("/") ? "/" : columns[45],
                        //StatusHum2 = columns[46].Contains("/") ? "/" : columns[46],
                        StatusDirect1 = columns[47].Contains("/") ? "/" : columns[47],
                        StatusDirect2 = columns[48].Contains("/") ? "/" : columns[48],
                        StatusSpeed1 = columns[49].Contains("/") ? "/" : columns[49],
                        StatusSpeed2 = columns[50].Contains("/") ? "/" : columns[50],
                        StatusSpeedNasal = columns[51].Contains("/") ? "/" : columns[51],
                        StatusDirectNasal = columns[52].Contains("/") ? "/" : columns[52],
                        StatusPressure1 = columns[53].Contains("/") ? "/" : columns[53],
                        StatusDVGO = columns[54].Contains("/") ? "/" : columns[54],

                        AmountClouds = columns[55].Contains("/") ? "Н.Д." : columns[55],
                        StatusDMDV = columns[56].Contains("/") ? "/" : columns[56],
                        Temp_1mid1 = columns[57].Contains("/") ? "Н.Д." : columns[57],
                        Hum_1mid1 = columns[58].Contains("/") ? "Н.Д." : columns[58],
                        Hum_1mid2 = columns[59].Contains("/") ? "Н.Д." : columns[59],
                        Temp_1mid2 = columns[60].Contains("/") ? "Н.Д." : columns[60],
                        Speed_2K1 = columns[61].Contains("/") ? "Н.Д." : columns[61],
                        Speed_2K2 = columns[62].Contains("/") ? "Н.Д." : columns[62],
                        Direction_2Kmid1 = columns[63].Contains("/") ? "Н.Д." : columns[63],
                        Direction_2Kmid2 = columns[64].Contains("/") ? "Н.Д." : columns[64],

                        Speed_Knasal = columns[65].Contains("/") ? "Н.Д." : columns[65],
                        Direction_Knasal = columns[66].Contains("/") ? "Н.Д." : columns[66],
                        Direction_2Knasalmid = columns[67].Contains("/") ? "Н.Д." : columns[67],
                        Direction_2Knasalmin = columns[68].Contains("/") ? "Н.Д." : columns[68],
                        Direction_2Knasalmax = columns[69].Contains("/") ? "Н.Д." : columns[69],
                        Direction_10Knasalmid = columns[70].Contains("/") ? "Н.Д." : columns[70],
                        Direction_10Knasalmin = columns[71].Contains("/") ? "Н.Д." : columns[71],
                        Direction_10Knasalmax = columns[72].Contains("/") ? "Н.Д." : columns[72],
                        Speed_2Knasalmid = columns[73].Contains("/") ? "Н.Д." : columns[73],
                        Speed_2Knasalmin = columns[74].Contains("/") ? "Н.Д." : columns[74],
                        Speed_2Knasalmax = columns[75].Contains("/") ? "Н.Д." : columns[75],
                        Speed_10Knasalmid = columns[76].Contains("/") ? "Н.Д." : columns[76],
                        Speed_10Knasalmin = columns[77].Contains("/") ? "Н.Д." : columns[77],
                        Speed_10Knasalmax = columns[78].Contains("/") ? "Н.Д." : columns[78],

                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return data;
        }
    }
}
