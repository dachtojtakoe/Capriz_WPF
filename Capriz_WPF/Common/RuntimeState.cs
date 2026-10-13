using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capriz_WPF.Common
{
    public static class RuntimeState
    {
        /// <summary>
        /// true — запись данных в БД и лог-файл приостановлена.
        /// Устанавливается на время копирования данных на USB.
        /// </summary>
        public static volatile bool IsDataWritingPaused = false;
    }
}

