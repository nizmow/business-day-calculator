using System;

namespace BusinessDaysBetween.Business.ValueObjects
{
    /// <summary>
    /// Tag interface for holidays
    /// </summary>
    public interface IHoliday
    {
        (bool present, DateTime date) GetDateForYear(int year);
    }
}