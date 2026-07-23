using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capriz_WPF.Data
{
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
