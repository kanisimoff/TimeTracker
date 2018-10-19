using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Logic
{
    public static class TimespanExtensions
    {
        public static string HoursFormat(this TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalHours}:{timeSpan:mm}:{timeSpan:ss}";
        }
    }
}
