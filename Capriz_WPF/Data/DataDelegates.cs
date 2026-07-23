using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capriz_WPF.Data
{
    class DataDelegates
    {
        public delegate void MyEventStr(string data);
        public static MyEventStr EventHandlerStr;

        public delegate void WriteFEventStr(string data);
        public static WriteFEventStr WriteFHandlerStr;

        public delegate void MyEventStrParam(string data);
        public static MyEventStrParam EventHandlerStrParam;
    }
}
