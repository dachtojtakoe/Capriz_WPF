using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Capriz_WPF.Data
{
    public class DataLite
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string PressureGPa { get; set; }
        public string PressureRtSt { get; set; }

        public string Speed_K { get; set; } //[6] //float
        public string Direction_K { get; set; } //[7] //int
        public string Speed_I { get; set; } //[8] //float  
        public string Direction_I { get; set; } //[9] //int

        public string ShipCourse { get; set; }
        public string ShipSpeed { get; set; } // М/с 


        public string BarTend { get; set; }
        public string Trend { get; set; }

        public bool MWVDone { get; set; }
        public bool XDRDone { get; set; }
        public bool VTGDone { get; set; }

        public int NoMWV { get; set; }
        public int NoXDR { get; set; }
        public int NoVTG { get; set; }

        public string Direction_2Kmid { get; set; } //[10] //int
        public string Direction_2Kmin { get; set; } //[11] //int
        public string Direction_2Kmax { get; set; } //[12] //int
        public string Direction_10Kmid { get; set; } //[13] //int
        public string Direction_10Kmin { get; set; } //[14] //int
        public string Direction_10Kmax { get; set; } //[15] //int

        public string Direction_2Imid { get; set; } //[16] //int
        public string Direction_2Imin { get; set; } //[17] //int
        public string Direction_2Imax { get; set; } //[18] //int
        public string Direction_10Imid { get; set; } //[19] //int
        public string Direction_10Imin { get; set; } //[20] //int
        public string Direction_10Imax { get; set; } //[21] //int

        public string Speed_2Kmid { get; set; } //[22] //float
        public string Speed_2Kmin { get; set; } //[23] //float
        public string Speed_2Kmax { get; set; } //[24] //float
        public string Speed_10Kmid { get; set; } //[25] //float
        public string Speed_10Kmin { get; set; } //[26] //float
        public string Speed_10Kmax { get; set; } //[27] //float

        public string Speed_2Imid { get; set; } //[28] //float
        public string Speed_2Imin { get; set; } //[29] //float
        public string Speed_2Imax { get; set; } //[30] //float
        public string Speed_10Imid { get; set; } //[31] //float
        public string Speed_10Imin { get; set; } //[32] //float
        public string Speed_10Imax { get; set; } //[33] //float

        private List<double> speedKList = new List<double>();
        private List<double> directionKList = new List<double>();
        private List<double> speedIList = new List<double>();
        private List<double> directionIList = new List<double>();

        public double SpeedKList
        {
            get
            {
                return speedKList.Last();
            }
            set
            {
                int count = speedKList.Count;
                if (count > 600)
                    speedKList.RemoveAt(0);
                speedKList.Add(value);
                if (count > 120)
                {
                    Speed_2Kmin = Math.Round(speedKList.GetRange(count - 120, 120).Min(), 2).ToString();
                    Speed_2Kmid = Math.Round(speedKList.GetRange(count - 120, 120).Average(), 2).ToString();
                    Speed_2Kmax = Math.Round(speedKList.GetRange(count - 120, 120).Max(), 2).ToString(); 
                }
                else
                {
                    Speed_2Kmin = "Н.Д.";
                    Speed_2Kmid = "Н.Д.";
                    Speed_2Kmax = "Н.Д.";
                }
                if (count > 600)
                {
                    Speed_10Kmin = Math.Round(speedKList.Min(), 2).ToString();
                    Speed_10Kmid = Math.Round(speedKList.Average(), 2).ToString();
                    Speed_10Kmax = Math.Round(speedKList.Max(), 2).ToString();
                }
                else
                {
                    Speed_10Kmin = "Н.Д.";
                    Speed_10Kmid = "Н.Д.";
                    Speed_10Kmax = "Н.Д.";
                }
            }
        }

        public double DirectionKList
        {
            get
            {
                return directionKList.Last();
            }
            set
            {
                int count = directionKList.Count;
                if (count > 600)
                    directionKList.RemoveAt(0);
                directionKList.Add(value);
                if (count > 120)
                {
                    Direction_2Kmin = Math.Round(directionKList.GetRange(count - 120, 120).Min(), 2).ToString();
                    Direction_2Kmid = Math.Round(directionKList.GetRange(count - 120, 120).Average(), 2).ToString();
                    Direction_2Kmax = Math.Round(directionKList.GetRange(count - 120, 120).Max(), 2).ToString();
                }
                else
                {
                    Direction_2Kmin = "Н.Д.";
                    Direction_2Kmid = "Н.Д.";
                    Direction_2Kmax = "Н.Д.";
                }
                if (count > 600)
                {
                    Direction_10Kmin = Math.Round(directionKList.Min(), 2).ToString();
                    Direction_10Kmid = Math.Round(directionKList.Average(), 2).ToString();
                    Direction_10Kmax = Math.Round(directionKList.Max(), 2).ToString();
                }
                else
                {
                    Direction_10Kmin = "Н.Д.";
                    Direction_10Kmid = "Н.Д.";
                    Direction_10Kmax = "Н.Д.";
                }
            }
        }

        public double SpeedIList
        {
            get
            {
                return speedIList.Last();
            }
            set
            {
                int count = speedIList.Count;
                if (count > 600)
                    speedIList.RemoveAt(0);
                speedIList.Add(value);
                if (count > 120)
                {
                    Speed_2Imin = Math.Round(speedIList.GetRange(count - 120, 120).Min(), 2).ToString();
                    Speed_2Imid = Math.Round(speedIList.GetRange(count - 120, 120).Average(), 2).ToString();
                    Speed_2Imax = Math.Round(speedIList.GetRange(count - 120, 120).Max(), 2).ToString();
                }
                else
                {
                    Speed_2Imin = "Н.Д.";
                    Speed_2Imid = "Н.Д.";
                    Speed_2Imax = "Н.Д.";
                }
                if (count > 600)
                {
                    Speed_10Imin = Math.Round(speedIList.Min(), 2).ToString();
                    Speed_10Imid = Math.Round(speedIList.Average(), 2).ToString();
                    Speed_10Imax = Math.Round(speedIList.Max(), 2).ToString();
                }
                else
                {
                    Speed_10Imin = "Н.Д.";
                    Speed_10Imid = "Н.Д.";
                    Speed_10Imax = "Н.Д.";
                }
            }
        }

        public double DirectionIList
        {
            get
            {
                return directionIList.Last();
            }
            set
            {
                int count = directionIList.Count;
                if (count > 600)
                    directionIList.RemoveAt(0);
                directionIList.Add(value);
                if (count > 120)
                {
                    Direction_2Imin = Math.Round(directionIList.GetRange(count - 120, 120).Min(), 2).ToString();
                    Direction_2Imid = Math.Round(directionIList.GetRange(count - 120, 120).Average(), 2).ToString();
                    Direction_2Imax = Math.Round(directionIList.GetRange(count - 120, 120).Max(), 2).ToString();
                }
                else
                {
                    Direction_2Imin = "Н.Д.";
                    Direction_2Imid = "Н.Д.";
                    Direction_2Imax = "Н.Д.";
                }
                if (count > 600)
                {
                    Direction_10Imin = Math.Round(directionIList.Min(), 2).ToString();
                    Direction_10Imid = Math.Round(directionIList.Average(), 2).ToString();
                    Direction_10Imax = Math.Round(directionIList.Max(), 2).ToString();
                }
                else
                {
                    Direction_10Imin = "Н.Д.";
                    Direction_10Imid = "Н.Д.";
                    Direction_10Imax = "Н.Д.";
                }
            }
        }


        public void Clear()
        {
            Date = string.Empty;
            Time = string.Empty;
            Temperature = string.Empty;
            Humidity = string.Empty;
            PressureGPa = string.Empty;
            Speed_K = string.Empty;
            Direction_K = string.Empty;
            Speed_I = string.Empty;
            Direction_I = string.Empty;
            BarTend = string.Empty;
            Trend = string.Empty;

            MWVDone = false;
            XDRDone = false;
            VTGDone = false;

            NoMWV = 0;
            NoXDR = 0;
        }
    }

    public class Data
    {
        //////время
        public string Date { get; set; }//[0]

        //////время
        public string Time { get; set; }//[0]

        public string Temperature { get; set; } //[0] //float
        public string Humidity { get; set; } //[1] //int
        public string PressureGPa { get; set; } //[2] //float
        public string PressureRtSt { get; set; } //[3] //float
        public string BarTend { get; set; } //[4] //float
        public string Trend { get; set; } //[5] //int

        public string Speed_K { get; set; } //[6] //float
        public string Direction_K { get; set; } //[7] //int
        public string Speed_I { get; set; } //[8] //float  
        public string Direction_I { get; set; } //[9] //int

        public string Direction_2Kmid { get; set; } //[10] //int
        public string Direction_2Kmin { get; set; } //[11] //int
        public string Direction_2Kmax { get; set; } //[12] //int
        public string Direction_10Kmid { get; set; } //[13] //int
        public string Direction_10Kmin { get; set; } //[14] //int
        public string Direction_10Kmax { get; set; } //[15] //int

        public string Direction_2Imid { get; set; } //[16] //int
        public string Direction_2Imin { get; set; } //[17] //int
        public string Direction_2Imax { get; set; } //[18] //int
        public string Direction_10Imid { get; set; } //[19] //int
        public string Direction_10Imin { get; set; } //[20] //int
        public string Direction_10Imax { get; set; } //[21] //int

        public string Speed_2Kmid { get; set; } //[22] //float
        public string Speed_2Kmin { get; set; } //[23] //float
        public string Speed_2Kmax { get; set; } //[24] //float
        public string Speed_10Kmid { get; set; } //[25] //float
        public string Speed_10Kmin { get; set; } //[26] //float
        public string Speed_10Kmax { get; set; } //[27] //float

        public string Speed_2Imid { get; set; } //[28] //float
        public string Speed_2Imin { get; set; } //[29] //float
        public string Speed_2Imax { get; set; } //[30] //float
        public string Speed_10Imid { get; set; } //[31] //float
        public string Speed_10Imin { get; set; } //[32] //float
        public string Speed_10Imax { get; set; } //[33] //float

        public string Visibility10 { get; set; } //[34] //int
        public string Visibility1 { get; set; } //[35] //int

        public string AmountPrecipitation { get; set; } //[36] //float

        public string ShipSpeed { get; set; } //[37] //float
        public string CourseShip { get; set; } //[38] //int
        public string NGO1 { get; set; } //[39] //int
        public string NGO2 { get; set; } //[40] //int
        public string NGO3 { get; set; } //[41] //int

        public string StatusTemp1 { get; set; } //[42] //int
        public string StatusTemp2 { get; set; } //[43] //int
        public string StatusHum1 { get; set; } //[44] //int
        public string StatusHum2 { get; set; } //[45] //int
        public string StatusDirect1 { get; set; } //[46] //int
        public string StatusDirect2 { get; set; } //[47] //int
        public string StatusSpeed1 { get; set; } //[48] //int
        public string StatusSpeed2 { get; set; } //[49] //int
        public string StatusSpeedNasal { get; set; } //[50] //int
        public string StatusDirectNasal { get; set; } //[51] //int
        public string StatusPressure { get; set; } //[52] //int
        public string StatusDVGO { get; set; } //[53] //char
        public string AmountClouds { get; set; } //[54] //int
        public string StatusDMDV { get; set; } //[55] //int

        public string Temp_1mid1 { get; set; } //[56] //int
        public string Hum_1mid1 { get; set; } //[57] //int
        public string Hum_1mid2 { get; set; } //[58] //int
        public string Temp_1mid2 { get; set; } //[59] //int



        public string Speed_2K1 { get; set; } //[60] //float
        public string Speed_2K2 { get; set; } //[61] //float

        public string Direction_2Kmid1 { get; set; } //[62] //int
        public string Direction_2Kmid2 { get; set; } //[63] //int


        public string Speed_Knasal { get; set; } //[64] //int
        public string Direction_Knasal { get; set; } //[65] //int


        public string Direction_2Knasalmid { get; set; } //[66] //int
        public string Direction_2Knasalmin { get; set; } //[67] //int
        public string Direction_2Knasalmax { get; set; } //[68] //int
        public string Direction_10Knasalmid { get; set; } //[69] //int
        public string Direction_10Knasalmin { get; set; } //[70] //int
        public string Direction_10Knasalmax { get; set; } //[71] //int

        public string Speed_2Knasalmid { get; set; } //[72] //int
        public string Speed_2Knasalmin { get; set; } //[73] //int
        public string Speed_2Knasalmax { get; set; } //[74] //int
        public string Speed_10Knasalmid { get; set; } //[75] //int
        public string Speed_10Knasalmin { get; set; } //[76] //int
        public string Speed_10Knasalmax { get; set; } //[77] //int
    }


}
