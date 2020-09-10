using BusinessDaysBetween.Business.Infrastructure;
using BusinessDaysBetween.Business.ValueObjects;

namespace BusinessDaysBetween.Business.Application
{
    public interface IHolidayFactory
    {
        IHoliday CreateHoliday(HolidayDto holidayDto);
    }
}