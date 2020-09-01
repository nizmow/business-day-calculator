namespace BusinessDaysBetween.Business.ValueObjects
{
    public enum HolidayType
    {
        /// <summary>
        /// Holiday occurs on the same date every year, even if it's otherwise a weekend
        /// </summary>
        Fixed,
        
        /// <summary>
        /// Holiday occurs on the same date every year, but rolls over to Monday if it's a weekend
        /// </summary>
        RollsToMonday,
        
        /// <summary>
        /// Holiday occurs on a particular day of the month, eg: the second Monday of April
        /// </summary>
        ParticularDayOfMonth,
    }
}