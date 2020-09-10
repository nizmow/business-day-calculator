using System;
using BusinessDaysBetween.Business.Infrastructure;
using BusinessDaysBetween.Business.ValueObjects;
// ReSharper disable PossibleInvalidOperationException

namespace BusinessDaysBetween.Business.Application
{
    public class HolidayFactory : IHolidayFactory
    {
        public IHoliday CreateHoliday(HolidayDto holidayDto)
        {
            return holidayDto.Type switch
            {
                HolidayType.ParticularDayOfMonth =>
                    new DayOfMonthHoliday(
                        holidayDto.ApplicableMonth.Value,
                        holidayDto.ApplicableDay.Value,
                        holidayDto.OccurenceInMonth.Value),
                var x when x == HolidayType.Fixed || x == HolidayType.RollsToMonday =>
                    new FixedHoliday(
                        x == HolidayType.RollsToMonday,
                        holidayDto.Date.Value),
                _ => throw new InvalidOperationException($"Unknown holiday type {holidayDto.Type}")
            };
        }
    }
}