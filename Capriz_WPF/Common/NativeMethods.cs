using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Capriz_WPF.Common
{
    internal static class NativeMethods
    {
        //Для запрета ухода в спящий режим
        [DllImport("Kernel32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE state);

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 2,
            ES_SYSTEM_REQUIRED = 1,
            ES_AWAYMODE_REQUIRED = 0x00000040
        }
    }
}
