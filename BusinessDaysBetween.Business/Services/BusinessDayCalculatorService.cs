using System;
using System.Collections.Generic;
using System.Linq;
using BusinessDaysBetween.Business.Extensions;
using BusinessDaysBetween.Business.ValueObjects;

namespace BusinessDaysBetween.Business.Services
{
    /// <summary>
    /// A stateless "service class" that calculates the business days between two dates
    /// </summary>
    public class BusinessDayCalculatorService : IBusinessDayCalculatorService
    {
        /// <summary>
        /// This is the high speed version. It doesn't work.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public int CalculateBusinessDaysBetween(DateTime startDate, DateTime endDate,
            IEnumerable<IHoliday> holidays = null)
        {
            // initialise optional values so we don't have to worry -- I'd prefer not to use nulls but that's another story
            holidays ??= new List<IHoliday>();

            // adjust start date and end date to be inclusive to work better with C# date arithmetic
            startDate = startDate.AddDays(1);
            endDate = endDate.AddDays(-1);

            // graciously deal with some edge cases
            if (startDate > endDate || startDate == endDate)
            {
                return 0;
            }

            // trim start date to week start
            startDate = startDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => startDate.AddDays(2),
                DayOfWeek.Sunday => startDate.AddDays(1),
                _ => startDate
            };
            // trim end date to week end
            endDate = endDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => endDate.AddDays(-1),
                DayOfWeek.Sunday => endDate.AddDays(-2),
                _ => endDate
            };

            // add one here because we're dealing now with fully inclusive dates, and Subtract().Days doesn't do what
            // we want.
            var daysDifference = endDate.Subtract(startDate).Days + 1;

            // in this case the total of business days is 5 for every week of 7, plus whatever is left over!
            var numberOfBusinessDays = daysDifference / 7 * 5 + daysDifference % 7;
            // BUT, detect if we've crossed a weekend and remove the weekend
            // we subtract one here because of our adjustment above
            if (endDate.DayOfWeek < startDate.DayOfWeek - 1)
            {
                numberOfBusinessDays -= 2;
            }

            // remove any holidays detected!
            for (var year = startDate.Year; year <= endDate.Year; year++)
            {
                // this stops a warning about a captured variable being modified, even though it doesn't matter right
                // now it might in the future, best to be explicit
                var queryYear = year;
                var weekdayHolidays = holidays
                    .Select(h => h.GetDateForYear(queryYear))
                    .Where(hr => hr.present)
                    .Select(hr => hr.date)
                    .Where(d => !d.IsWeekend());

                // check how many of the holidays overlap our date range
                var numberOfOverlappingHolidays = weekdayHolidays.Count(w => IsBetween(w, startDate, endDate));

                // adjust result accordingly
                numberOfBusinessDays -= numberOfOverlappingHolidays;
            }

            return numberOfBusinessDays;
        }

        private bool IsBetween(DateTime candidateDate, DateTime startDate, DateTime endDate) => candidateDate >= startDate && candidateDate <= endDate;
    }
}
