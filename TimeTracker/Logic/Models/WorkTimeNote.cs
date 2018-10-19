using System;
using System.Windows.Forms;

namespace TimeTracker.Logic.Models
{
    /// <summary>
    /// Note for working day
    /// </summary>
    public struct WorkTimeNote
    {
        private DateTime _endDateTime;
        private TimeSpan baseWorkDayDuration;

        /// <summary>
        /// Business Day Start DateTime
        /// </summary>
        public DateTime BeginDateTime { get; set; }

        /// <summary>
        /// Business Day End DateTime
        /// </summary>
        public DateTime EndDateTime {
            get => _endDateTime;
            set
            {
                _endDateTime = value;
                ResetStartOfDay();
            }
        }

        /// <summary>
        /// Duration of the working day
        /// </summary>
        public TimeSpan Duration => EndDateTime.ToUniversalTime() - BeginDateTime.ToUniversalTime();

        /// <summary>
        /// Reset date if day changed
        /// </summary>
        private void ResetStartOfDay()
        {
            if (BeginDateTime.Date != EndDateTime.Date)
                BeginDateTime = EndDateTime;
        }

        /// <summary>
        /// Calculate work time
        /// </summary>
        public TimeSpan CalculateDayWorkTime
        {
            get
            {
                var holydayList = new HolidayList();
                if ( holydayList.Holidays.Contains(DateTime.Now.Date))
                    return new TimeSpan(0, 0, 0, 0) - Duration;
                if (holydayList.ShortDays.Contains(DateTime.Now.Date))
                    return new TimeSpan(0, SettingsDuration.ShortDayDurationHours, 0, 0) - Duration;
                return new TimeSpan(0, SettingsDuration.WorkDayDurationHours, 0, 0) - Duration;
            }
        }
    }
}
