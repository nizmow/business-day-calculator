using System;

namespace BusinessDaysBetween.Business.ValueObjects
{
    /// <summary>
    /// Any kind of holiday with a single date
    /// </summary>
    public interface IHoliday
    {
        /// <summary>
        /// Calculate the date this holiday falls on for the given year.
        /// </summary>
        /// <param name="year">Year on which to calculate</param>
        /// <returns>
        /// Present is true if the holiday falls on this year, and the result will be returned as date. False and with
        /// no meaningful date if it doesn't fall on this year.
        /// </returns>
        (bool present, DateTime date) GetDateForYear(int year);
    }
}