using System;

namespace BusinessDaysBetween.Business.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekend(this DateTime dateToTest) => dateToTest.DayOfWeek == DayOfWeek.Saturday || dateToTest.DayOfWeek == DayOfWeek.Sunday; 
    }
}
