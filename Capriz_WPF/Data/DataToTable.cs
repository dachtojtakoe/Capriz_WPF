using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Capriz_WPF.Data
{
    static class DataToTable
    {
        public static DataTable CreateDt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Temperature", typeof(string));
            dt.Columns.Add("Humidity", typeof(string));
            dt.Columns.Add("PressureGPa", typeof(string));
            dt.Columns.Add("PressureRtSt", typeof(string));
            dt.Columns.Add("BarTend", typeof(string));
            dt.Columns.Add("Trend", typeof(string));
            dt.Columns.Add("Speed_K", typeof(string));
            dt.Columns.Add("Direction_K", typeof(string));
            dt.Columns.Add("Speed_I", typeof(string));
            dt.Columns.Add("Direction_I", typeof(string));
            dt.Columns.Add("Direction_2Kmid", typeof(string));
            dt.Columns.Add("Direction_2Kmin", typeof(string));
            dt.Columns.Add("Direction_2Kmax", typeof(string));
            dt.Columns.Add("Direction_10Kmid", typeof(string));
            dt.Columns.Add("Direction_10Kmin", typeof(string));
            dt.Columns.Add("Direction_10Kmax", typeof(string));
            dt.Columns.Add("Direction_2Imid", typeof(string));
            dt.Columns.Add("Direction_2Imin", typeof(string));
            dt.Columns.Add("Direction_2Imax", typeof(string));
            dt.Columns.Add("Direction_10Imid", typeof(string));
            dt.Columns.Add("Direction_10Imin", typeof(string));
            dt.Columns.Add("Direction_10Imax", typeof(string));
            dt.Columns.Add("Speed_2Kmid", typeof(string));
            dt.Columns.Add("Speed_2Kmin", typeof(string));
            dt.Columns.Add("Speed_2Kmax", typeof(string));
            dt.Columns.Add("Speed_10Kmid", typeof(string));
            dt.Columns.Add("Speed_10Kmin", typeof(string));
            dt.Columns.Add("Speed_10Kmax", typeof(string));
            dt.Columns.Add("Speed_2Imid", typeof(string));
            dt.Columns.Add("Speed_2Imin", typeof(string));
            dt.Columns.Add("Speed_2Imax", typeof(string));
            dt.Columns.Add("Speed_10Imid", typeof(string));
            dt.Columns.Add("Speed_10Imin", typeof(string));
            dt.Columns.Add("Speed_10Imax", typeof(string));
            dt.Columns.Add("Visibility10", typeof(string));
            dt.Columns.Add("Visibility1", typeof(string));
            dt.Columns.Add("AmountPrecipitation", typeof(string));
            dt.Columns.Add("ShipSpeed", typeof(string));
            dt.Columns.Add("CourseShip", typeof(string));
            dt.Columns.Add("NGO1", typeof(string));
            dt.Columns.Add("NGO2", typeof(string));
            dt.Columns.Add("NGO3", typeof(string));
            dt.Columns.Add("StatusTemp1", typeof(string));
            dt.Columns.Add("StatusTemp2", typeof(string));
            dt.Columns.Add("StatusHum1", typeof(string));
            dt.Columns.Add("StatusHum2", typeof(string));
            dt.Columns.Add("StatusDirect1", typeof(string));
            dt.Columns.Add("StatusDirect2", typeof(string));
            dt.Columns.Add("StatusSpeed1", typeof(string));
            dt.Columns.Add("StatusSpeed2", typeof(string));
            dt.Columns.Add("StatusSpeedNasal", typeof(string));
            dt.Columns.Add("StatusDirectNasal", typeof(string));
            dt.Columns.Add("StatusDirectPressure", typeof(string));

            dt.Columns.Add("StatusDVGO", typeof(string));
            dt.Columns.Add("AmountClouds", typeof(string));
            dt.Columns.Add("StatusDMDV", typeof(string));
            dt.Columns.Add("Temp_1mid1", typeof(string));
            dt.Columns.Add("Hum_1mid1", typeof(string));
            dt.Columns.Add("Hum_1mid2", typeof(string));
            dt.Columns.Add("Temp_1mid2", typeof(string));
            dt.Columns.Add("Speed_2K1", typeof(string));
            dt.Columns.Add("Speed_2K2", typeof(string));

            dt.Columns.Add("Direction_2Kmid1", typeof(string));
            dt.Columns.Add("Direction_2Kmid2", typeof(string));
            dt.Columns.Add("Speed_Knasal", typeof(string));
            dt.Columns.Add("Direction_Knasal", typeof(string));

            dt.Columns.Add("Direction_2Knasalmid", typeof(string));
            dt.Columns.Add("Direction_2Knasalmin", typeof(string));
            dt.Columns.Add("Direction_2Knasalmax", typeof(string));
            dt.Columns.Add("Direction_10Knasalmid", typeof(string));
            dt.Columns.Add("Direction_10Knasalmin", typeof(string));
            dt.Columns.Add("Direction_10Knasalmax", typeof(string));

            dt.Columns.Add("Speed_2Knasalmid", typeof(string));
            dt.Columns.Add("Speed_2Knasalmin", typeof(string));
            dt.Columns.Add("Speed_2Knasalmax", typeof(string));
            dt.Columns.Add("Speed_10Knasalmid", typeof(string));
            dt.Columns.Add("Speed_10Knasalmin", typeof(string));
            dt.Columns.Add("Speed_10Knasalmax", typeof(string));

            return dt;
        }

        public static DataTable CreateDtGraphic()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(string));
            //dt.Columns.Add("Date", typeof(string));
            //dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Temperature", typeof(string));
            dt.Columns.Add("Humidity", typeof(string));
            dt.Columns.Add("PressureGPa", typeof(string));
            dt.Columns.Add("Speed_I", typeof(string));
            dt.Columns.Add("Visibility10", typeof(string));
            return dt;
        }

        public static DataTable CopyDt(DataTable dtFromFile)
        {
            DataTable dt = CreateDtGraphic();

            foreach (DataRow drF in dtFromFile.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["DateTime"] = Convert.ToDateTime(drF["DateTime"]).ToString("dd.MM.yyyy HH:mm:ss");
                    
                //dr["Date"] = Convert.ToDateTime(drF["DateTime"]).ToString("dd.MM.yyyy");
                //dr["Time"] = Convert.ToDateTime(drF["DateTime"]).ToString("HH:mm:ss");
                dr["Temperature"] = drF["Temperature"];
                dr["Humidity"] = drF["Humidity"];
                dr["PressureGPa"] = drF["PressureGPa"];
                dr["Speed_I"] = drF["Speed_I"];
                dr["Visibility10"] = drF["Visibility10"];
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable CopyDtChart(DataTable dtFromFile, string nameParametr)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add(nameParametr, typeof(double));

            foreach (DataRow drF in dtFromFile.Rows)
            {
                DataRow dr = dt.NewRow();
                    dr["Date"] = drF["Date"];
                dr["Time"] = drF["Time"];
                dr[nameParametr] = drF[nameParametr].ToString() == "Н.Д." ? -999 :  drF[nameParametr]; // !!! Так или нет
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable CopyDtChart2(DataTable dtFromFile, string nameParametr)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add(nameParametr, typeof(string));

            foreach (DataRow drF in dtFromFile.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["Date"] = drF["Date"];
                dr["Time"] = drF["Time"];
                dr[nameParametr] = drF[nameParametr];
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static List<Data> DataToList(string allData)
        {
            List<Data> data = new List<Data>();
            try
            {
                var columns = allData.Split('\t', '\x02', '\x03', '*');

                if (columns.GetLength(0) < 81)
                    return null;

                data.Add(new Data
                {
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
                    StatusTemp2 = columns[44].Contains("/") ? "/" : columns[44],
                    StatusHum1 = columns[45].Contains("/") ? "/" : columns[45],
                    StatusHum2 = columns[46].Contains("/") ? "/" : columns[46],
                    StatusDirect1 = columns[47].Contains("/") ? "/" : columns[47],
                    StatusDirect2 = columns[48].Contains("/") ? "/" : columns[48],
                    StatusSpeed1 = columns[49].Contains("/") ? "/" : columns[49],
                    StatusSpeed2 = columns[50].Contains("/") ? "/" : columns[50],
                    StatusSpeedNasal = columns[51].Contains("/") ? "/" : columns[51],
                    StatusDirectNasal = columns[52].Contains("/") ? "/" : columns[52],
                    StatusPressure = columns[53].Contains("/") ? "/" : columns[53],
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
                    Speed_10Knasalmax = columns[78].Contains("/") ? "Н.Д." : columns[78]

                });
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return data;
        }
    }
}
