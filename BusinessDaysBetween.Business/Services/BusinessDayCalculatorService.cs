using System;

namespace BusinessDaysBetween.Business.Services
{
    /// <summary>
    /// A stateless "service class" that calculates the business days between two dates
    /// </summary>
    public class BusinessDayCalculatorService
    {
        public int CalculateBusinessDaysBetween(DateTime startDate, DateTime endDate)
        {
            // graciously deal with some edge cases
            if (startDate > endDate || startDate == endDate)
            {
                return 0;
            }
            
            // adjust start date and end date to be inclusive to work better with C# date arithmetic
            startDate = startDate.AddDays(1);
            endDate = endDate.AddDays(-1);

            // we can use some fancy enumerable extensions here but why make things hard to read?
            var numberOfBusinessDays = 0;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (IsWeekend(date))
                {
                    continue;
                }

                numberOfBusinessDays++;
            }

            return numberOfBusinessDays;
        }

        private bool IsWeekend(DateTime dateToTest) => dateToTest.DayOfWeek == DayOfWeek.Saturday || dateToTest.DayOfWeek == DayOfWeek.Sunday;
    }
}