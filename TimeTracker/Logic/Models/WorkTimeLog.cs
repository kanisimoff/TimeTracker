using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Logic.Models
{
    /// <summary>
    /// Class model for login work time
    /// </summary>
    public class WorkTimeLog
    {
        private HolidayList holidayList;

        public WorkTimeLog()
        {
            LogNotes = new Dictionary<DateTime, WorkTimeNote>();
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version = fvi.FileVersion;

            holidayList = new HolidayList();
        }

        /// <summary>
        /// Log version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Log note
        /// </summary>
        public Dictionary<DateTime, WorkTimeNote> LogNotes { get; set; }

        /// <summary>
        /// The number of hours you need to work per week
        /// </summary>
        public TimeSpan LeftCurrentWeek
        {
            get
            {
                var startDateWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                var endDateWeek = startDateWeek.AddDays(6);
                var businessHoursCount = startDateWeek.BusinessHours(endDateWeek, holidayList.Holidays.ToArray(), holidayList.ShortDays.ToArray());

                return new TimeSpan(0, businessHoursCount, 0, 0) - DurationCurrentWeek;
            }
        }

        /// <summary>
        /// Hours worked in the current week
        /// </summary>
        public TimeSpan DurationCurrentWeek
        {
            get
            {
                var startDateWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                return LogNotes.Where(l => l.Key >= startDateWeek)
                    .Aggregate(new TimeSpan(), (current, keyValuePair) => current + keyValuePair.Value.Duration);
            }
        }

        /// <summary>
        /// The number of hours you need to work per month
        /// </summary>
        public TimeSpan LeftCurrentMonth
        {
            get
            {
                var dateTime = DateTime.Now;
                var startDateMonth = new DateTime(dateTime.Year, dateTime.Month, 01);
                var endDateMonth = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month)); ;
                var businessHoursCount = startDateMonth.BusinessHours(endDateMonth, holidayList.Holidays.ToArray(), holidayList.ShortDays.ToArray());

                var result = new TimeSpan(0, businessHoursCount, 0, 0) - DurationCurrentMonth;

                return result;
            }
        }

        /// <summary>
        /// Number of hours worked in the current month
        /// </summary>
        public TimeSpan DurationCurrentMonth
        {
            get
            {
                var startDateWeek = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                return LogNotes.Where(l => l.Key >= startDateWeek)
                    .Aggregate(new TimeSpan(), (current, keyValuePair) => current + keyValuePair.Value.Duration);
            }
        }

    }
}
