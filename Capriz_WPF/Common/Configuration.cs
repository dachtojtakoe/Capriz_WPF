using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        string dtvv = "1";
        public string DTVV { get => dtvv; set => dtvv = value; }

        //string dtvv2 = "1";
        //public string DTVV2 { get => dtvv2; set => dtvv2 = value; }


        string dad1 = "1";
        public string DAD1 { get => dad1; set => dad1 = value; }

        string dad2 = "1";
        public string DAD2 { get => dad2; set => dad2 = value; }

        string dvgo = "1";
        public string DVGO { get => dvgo; set => dvgo = value; }

        string dmdv = "1";
        public string DMDV { get => dmdv; set => dmdv = value; }


        string height = "0";
        public string HEIGHT { get => height; set => height = value; }

        string navigation = "0";
        public string NAVIGATION {  get => navigation; set => navigation = value; }

        public void SetData(List<string> data)
        {
            DSNV1 = data[0];
            DSNV2 = data[1];
            DSNV3 = data[2];
            DTVV = data[3];
            DAD1 = data[4];
            DAD2 = data[5];
            DVGO = data[6];
            DMDV = data[7];
            HEIGHT = data[8];
            NAVIGATION = data[9];
        }

        public List<string> GetData()
        {
            return new List<string> { DSNV1, DSNV2, DSNV3, DTVV, DAD1, DAD2, DVGO, DMDV, HEIGHT };
        }
    }
}
