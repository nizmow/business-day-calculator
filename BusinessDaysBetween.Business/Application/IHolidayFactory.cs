using BusinessDaysBetween.Business.Infrastructure;
using BusinessDaysBetween.Business.ValueObjects;

namespace BusinessDaysBetween.Business.Application
{
    /// <summary>
    /// Creates holiday objects from DTOs.
    /// </summary>
    public interface IHolidayFactory
    {
        /// <summary>
        /// Create a holiday from the given DTO.
        /// </summary>
        /// <param name="holidayDto">DTO to create. MUST FIRST BE VALIDATED.</param>
        /// <returns>Some kind of holiday.</returns>
        IHoliday CreateHoliday(HolidayDto holidayDto);
    }
}