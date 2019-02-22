using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Logic
{
    public static class DateService
    {
        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, DateTime[] bankHolidays = null, DateTime[] shortDateTimes = null)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday
                    ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday
                    ? 7 : (int)lastDay.DayOfWeek;

                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            if (bankHolidays != null && bankHolidays.Any())
            {
                foreach (DateTime bankHoliday in bankHolidays)
                {
                    DateTime bh = bankHoliday.Date;
                    if (firstDay <= bh && bh <= lastDay)
                        --businessDays;
                }
            }

            if (shortDateTimes != null && shortDateTimes.Any())
            {
                businessDays += shortDateTimes.Where(d => d >= firstDay && d <= lastDay)
                    .Count(shortDay => shortDay.DayOfWeek == DayOfWeek.Saturday || shortDay.DayOfWeek == DayOfWeek.Sunday);
            }

            return businessDays;
        }

        /// <summary>
        /// Get the start date of the week for a given date
        /// </summary>
        /// <param name="dt">Date for which you need to know the beginning of the week</param>
        /// <param name="startOfWeek">Day that starts the week</param>
        /// <returns>Week start date</returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Calculates number of business hours
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <param name="shortDateTimes">List of days with short work time excluding weekends</param>
        /// <returns>Number of business hours during the 'span'</returns>
        public static int BusinessHours(this DateTime firstDay, DateTime lastDay, DateTime[] bankHolidays = null, DateTime[] shortDateTimes = null)
        {
            var result = firstDay.BusinessDaysUntil(lastDay, bankHolidays, shortDateTimes) * SettingsDuration.WorkDayDurationHours;

            if (shortDateTimes != null)
            {
                var shortDays = shortDateTimes.Where(d => d >= firstDay && d <= lastDay).ToList();
                var intDays = shortDays.Count();
                result -= (SettingsDuration.WorkDayDurationHours - SettingsDuration.ShortDayDurationHours) * intDays;
                    
            }
            return result;
        }
    }
}
