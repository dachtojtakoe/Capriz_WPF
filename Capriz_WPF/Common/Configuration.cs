using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capriz_WPF.Common
{
    public class Configuration
    {
        string dsnv1 = "1";
        public string DSNV1 { get => dsnv1; set => dsnv1 = value; }

        string dsnv2 = "1";
        public string DSNV2 { get => dsnv2; set => dsnv2 = value; }

        string dsnv3 = "1";
        public string DSNV3 { get => dsnv3; set => dsnv3 = value; }

        string dtvv1 = "1";
        public string DTVV1 { get => dtvv1; set => dtvv1 = value; }

        string dtvv2 = "1";
        public string DTVV2 { get => dtvv2; set => dtvv2 = value; }


        string dad = "1";
        public string DAD { get => dad; set => dad = value; }

        string dvgo = "1";
        public string DVGO { get => dvgo; set => dvgo = value; }

        string dmdv = "1";
        public string DMDV { get => dmdv; set => dmdv = value; }

        string seeLevel = "1";
        public string SEELEVEL { get => seeLevel; set => seeLevel = value; }

        string height = "0";
        public string HEIGHT { get => height; set => height = value; }

        public void SetData(List<string> data)
        {
            DSNV1 = data[0];
            DSNV2 = data[1];
            DSNV3 = data[2];
            DTVV1 = data[3];
            DTVV2 = data[4];
            DAD = data[5];
            DVGO = data[6];
            DMDV = data[7];
            SEELEVEL = data[8];
            HEIGHT = data[9];
        }
        public List<string> GetData()
        {
            return new List<string> { DSNV1, DSNV2, DSNV3, DTVV1, DTVV2, DAD, DVGO, DMDV, SEELEVEL, HEIGHT };
        }
    }
}
