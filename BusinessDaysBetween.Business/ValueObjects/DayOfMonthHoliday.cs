using System;

namespace BusinessDaysBetween.Business.ValueObjects
{
    /// <summary>
    /// Holiday that falls on a particular day of the month, eg: the second Tuesday of November.
    /// </summary>
    public class DayOfMonthHoliday : IHoliday
    {
        public DayOfMonthHoliday(MonthOfYear applicableMonth, DayOfWeek applicableDay, int occurenceInMonth)
        {
            ApplicableMonth = applicableMonth;
            ApplicableDay = applicableDay;
            OccurenceInMonth = occurenceInMonth;
        }

        /// <summary>
        /// The month this holiday falls on.
        /// </summary>
        public MonthOfYear ApplicableMonth { get; }
        
        /// <summary>
        /// The day this holiday falls on.
        /// </summary>
        public DayOfWeek ApplicableDay { get; }
        
        /// <summary>
        /// The occurence in the month, eg: 2 for second Tuesday.
        /// </summary>
        public int OccurenceInMonth { get; }
        
        public (bool present, DateTime date) GetDateForYear(int year)
        {
            // short circuit and save some time -- there can be no instances of the 6th day!
            if (OccurenceInMonth > 5)
            {
                return (false, default);
            }

            var startOfMonth = new DateTime(year, (int) ApplicableMonth + 1, 1);
            while (startOfMonth.DayOfWeek != ApplicableDay)
            {
                startOfMonth = startOfMonth.AddDays(1);
            }

            startOfMonth = startOfMonth.AddDays((OccurenceInMonth - 1) * 7);

            if (startOfMonth.Month != (int) ApplicableMonth + 1)
            {
                // looks like this day doesn't happen this year!
                return (false, default);
            }
            
            return (true, startOfMonth);
        }
    }
}
