using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Logic.Models
{
    public class HolidayList
    {
        /// <summary>
        /// Russian holidays
        /// </summary>
        public List<DateTime> Holidays { get; set; }

        /// <summary>
        /// Russian shortened days
        /// </summary>
        public List<DateTime> ShortDays { get; set; }

        public HolidayList()
        {
            // 2018
            Holidays = new List<DateTime>();
            Holidays.Add(new DateTime(2018, 01, 01));
            Holidays.Add(new DateTime(2018, 01, 02));
            Holidays.Add(new DateTime(2018, 01, 03));
            Holidays.Add(new DateTime(2018, 01, 04));
            Holidays.Add(new DateTime(2018, 01, 05));
            Holidays.Add(new DateTime(2018, 01, 08));

            Holidays.Add(new DateTime(2018, 02, 23));

            Holidays.Add(new DateTime(2018, 03, 08));
            Holidays.Add(new DateTime(2018, 03, 09));

            Holidays.Add(new DateTime(2018, 04, 30));
            Holidays.Add(new DateTime(2018, 05, 01));
            Holidays.Add(new DateTime(2018, 05, 02));
            Holidays.Add(new DateTime(2018, 05, 09));

            Holidays.Add(new DateTime(2018, 06, 11));
            Holidays.Add(new DateTime(2018, 06, 12));

            Holidays.Add(new DateTime(2018, 11, 05));
            Holidays.Add(new DateTime(2018, 12, 31));


            ShortDays = new List<DateTime>();
            ShortDays.Add(new DateTime(2018, 02, 22));
            ShortDays.Add(new DateTime(2018, 03, 07));
            ShortDays.Add(new DateTime(2018, 04, 28));
            ShortDays.Add(new DateTime(2018, 05, 08));
            ShortDays.Add(new DateTime(2018, 06, 09));
            ShortDays.Add(new DateTime(2018, 12, 29));

            // 2019
            Holidays.Add(new DateTime(2019, 01, 01));
            Holidays.Add(new DateTime(2019, 01, 02));
            Holidays.Add(new DateTime(2019, 01, 03));
            Holidays.Add(new DateTime(2019, 01, 04));
            Holidays.Add(new DateTime(2019, 01, 07));
            Holidays.Add(new DateTime(2019, 01, 08));

            Holidays.Add(new DateTime(2019, 03, 08));

            Holidays.Add(new DateTime(2019, 05, 01));
            Holidays.Add(new DateTime(2019, 05, 02));
            Holidays.Add(new DateTime(2019, 05, 03));
            Holidays.Add(new DateTime(2019, 05, 09));
            Holidays.Add(new DateTime(2019, 05, 10));

            Holidays.Add(new DateTime(2019, 06, 12));

            Holidays.Add(new DateTime(2019, 11, 04));

            ShortDays.Add(new DateTime(2019, 02, 22));
            ShortDays.Add(new DateTime(2019, 03, 07));
            ShortDays.Add(new DateTime(2019, 04, 30));
            ShortDays.Add(new DateTime(2019, 05, 08));
            ShortDays.Add(new DateTime(2019, 06, 11));
            ShortDays.Add(new DateTime(2019, 12, 31));
        }
    }
}
