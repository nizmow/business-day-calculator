using System;
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
        public int CalculateBusinessDaysBetween(DateTime startDate, DateTime endDate, IEnumerable<Holiday> holidays = null)
        {
            // initialise optional values so we don't have to worry -- I'd prefer not to use nulls but that's another story
            holidays ??= new List<Holiday>();
            
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
            
            // remove any holidays detected!
            for (var year = startDate.Year; year <= endDate.Year; year++)
            {
                var weekdayHolidays = holidays.Select(h => GetHolidayDateForYear(h, year)).Where(d => !IsWeekend(d));
                
                // check how many of the holidays overlap our date range
                var numberOfOverlappingHolidays = weekdayHolidays.Count(w => IsBetween(w, startDate, endDate));
                
                // adjust result accordingly
                numberOfBusinessDays -= numberOfOverlappingHolidays;
            }
            
            // todo: performance optimisation -- divide out number of full weeks

            return numberOfBusinessDays;
        }

        public DateTime GetHolidayDateForYear(Holiday holiday, int year)
        {
            if (holiday.Type == HolidayType.Fixed)
            {
                return new DateTime(year, holiday.Date.Month, holiday.Date.Day);
            }

            if (holiday.Type == HolidayType.RollsToMonday)
            {
                var workingDateTime = new DateTime(year, holiday.Date.Month, holiday.Date.Day);
                if (!IsWeekend(workingDateTime))
                {
                    return workingDateTime;
                }

                // get days until Monday -- add 7 then mod 7 so we fix the enum wrapping
                // this seems like a bit of a hack because we assume things about the enumeration values, but for now
                // we can rely on the framework DayOfWeek enum?
                var daysUntilMonday = ((int) DayOfWeek.Monday - (int) workingDateTime.DayOfWeek + 7) % 7;

                return workingDateTime.AddDays(daysUntilMonday);
            }

            if (holiday.Type == HolidayType.ParticularDayOfMonth)
            {
                var startOfMonth = new DateTime(year, (int)holiday.ApplicableMonth + 1, 1);
                while (startOfMonth.DayOfWeek != holiday.ApplicableDay)
                {
                    startOfMonth = startOfMonth.AddDays(1);
                }
                startOfMonth = startOfMonth.AddDays((holiday.OccurenceInMonth - 1) * 7);
                
                // todo: big wide open hole, this month might not have this day! we need an optional return type
                return startOfMonth;
            }
            
            throw new NotImplementedException("Unhandled HolidayType enumeration value!");
        }

        private bool IsWeekend(DateTime dateToTest) => dateToTest.DayOfWeek == DayOfWeek.Saturday || dateToTest.DayOfWeek == DayOfWeek.Sunday;

        private bool IsBetween(DateTime candidateDate, DateTime startDate, DateTime endDate) =>
            candidateDate >= startDate && candidateDate <= endDate;
    }
}