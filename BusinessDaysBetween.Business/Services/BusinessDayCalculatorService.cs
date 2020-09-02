﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            IEnumerable<Holiday> holidays = null)
        {
            // initialise optional values so we don't have to worry -- I'd prefer not to use nulls but that's another story
            holidays ??= new List<Holiday>();

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
                    .Select(h => GetHolidayDateForYear(h, queryYear))
                    .Where(hr => hr.present)
                    .Select(hr => hr.date)
                    .Where(d => !IsWeekend(d));

                // check how many of the holidays overlap our date range
                var numberOfOverlappingHolidays = weekdayHolidays.Count(w => IsBetween(w, startDate, endDate));

                // adjust result accordingly
                numberOfBusinessDays -= numberOfOverlappingHolidays;
            }

            return numberOfBusinessDays;
        }

        public (bool present, DateTime date) GetHolidayDateForYear(Holiday holiday, int year)
        {
            if (holiday.Type == HolidayType.Fixed)
            {
                return (true, new DateTime(year, holiday.Date.Month, holiday.Date.Day));
            }

            if (holiday.Type == HolidayType.RollsToMonday)
            {
                var workingDateTime = new DateTime(year, holiday.Date.Month, holiday.Date.Day);
                if (!IsWeekend(workingDateTime))
                {
                    return (true, workingDateTime);
                }

                // get days until Monday -- add 7 then mod 7 so we fix the enum wrapping
                // this seems like a bit of a hack because we assume things about the enumeration values, but for now
                // we can rely on the framework DayOfWeek enum?
                var daysUntilMonday = ((int) DayOfWeek.Monday - (int) workingDateTime.DayOfWeek + 7) % 7;

                return (true, workingDateTime.AddDays(daysUntilMonday));
            }

            if (holiday.Type == HolidayType.ParticularDayOfMonth)
            {
                // short circuit and save some time -- there can be no instances of the 6th day!
                if (holiday.OccurenceInMonth > 5)
                {
                    return (false, default);
                }

                var startOfMonth = new DateTime(year, (int) holiday.ApplicableMonth + 1, 1);
                while (startOfMonth.DayOfWeek != holiday.ApplicableDay)
                {
                    startOfMonth = startOfMonth.AddDays(1);
                }

                startOfMonth = startOfMonth.AddDays((holiday.OccurenceInMonth - 1) * 7);

                if (startOfMonth.Month != (int) holiday.ApplicableMonth + 1)
                {
                    // looks like this day doesn't happen this year!
                    return (false, default);
                }
                return (true, startOfMonth);
            }

            // not the best way to handle a missing case, but it works for now
            throw new NotImplementedException($"Unhandled HolidayType enumeration value: '{holiday.Type}");
        }

        private bool IsWeekend(DateTime dateToTest) => dateToTest.DayOfWeek == DayOfWeek.Saturday || dateToTest.DayOfWeek == DayOfWeek.Sunday;

        private bool IsBetween(DateTime candidateDate, DateTime startDate, DateTime endDate) => candidateDate >= startDate && candidateDate <= endDate;
    }
}
