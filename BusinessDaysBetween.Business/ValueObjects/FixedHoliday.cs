using System;
using BusinessDaysBetween.Business.Extensions;

namespace BusinessDaysBetween.Business.ValueObjects
{
    public class FixedHoliday : IHoliday
    {
        public FixedHoliday(bool rollsToNextMonday, DateTime date)
        {
            RollsToNextMonday = rollsToNextMonday;
            Date = date;
        }

        public DateTime Date { get; }
        
        public bool RollsToNextMonday { get; }
        
        public (bool present, DateTime date) GetDateForYear(int year)
        {
            var workingDateTime = new DateTime(year, Date.Month, Date.Day);
            if (workingDateTime.IsWeekend() && RollsToNextMonday)
            {
                // get days until Monday -- add 7 then mod 7 so we fix the enum wrapping
                // this seems like a bit of a hack because we assume things about the enumeration values, but for now
                // we can rely on the framework DayOfWeek enum?
                var daysUntilMonday = ((int) DayOfWeek.Monday - (int) workingDateTime.DayOfWeek + 7) % 7;

                return (true, workingDateTime.AddDays(daysUntilMonday));
            }
            
            return (true, workingDateTime);
        }
    }
}
