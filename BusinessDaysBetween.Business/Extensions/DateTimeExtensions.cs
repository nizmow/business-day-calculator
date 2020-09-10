using System;

namespace BusinessDaysBetween.Business.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Does this date fall on a weekend?
        /// </summary>
        /// <param name="dateToTest">Date to test</param>
        /// <returns>True if the date is a Saturday or Sunday, false if not.</returns>
        public static bool IsWeekend(this DateTime dateToTest) => dateToTest.DayOfWeek == DayOfWeek.Saturday || dateToTest.DayOfWeek == DayOfWeek.Sunday; 
    }
}
