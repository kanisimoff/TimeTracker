using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace TimeTracker
{
    /// <summary>
    /// Customized working hours
    /// </summary>
    public static class SettingsDuration
    {
        /// <summary>
        /// The duration of the working day in hours
        /// </summary>
        public static int WorkDayDurationHours => 9;

        /// <summary>
        /// Duration of the shortened working day in hours
        /// </summary>
        public static int ShortDayDurationHours => 8;
    }
}
