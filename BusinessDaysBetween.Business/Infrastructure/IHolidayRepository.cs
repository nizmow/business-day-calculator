using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessDaysBetween.Business.Infrastructure
{
    /// <summary>
    /// Very simple holiday "repository" that only does one thing -- loads holidays.
    /// </summary>
    public interface IHolidayRepository
    {
        /// <summary>
        /// Load holidays from whatever data source.
        /// </summary>
        /// <returns>List of holidays</returns>
        Task<IEnumerable<HolidayDto>> LoadHolidays();
    }
}