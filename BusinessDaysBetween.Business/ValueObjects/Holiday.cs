using System;

namespace BusinessDaysBetween.Business.ValueObjects
{
    /// <summary>
    /// A particular holiday, based on holiday type
    /// </summary>
    /// <remarks>
    /// Hugely unhappy with the implementation of this, but rather than get into inheritance I'll leave it as it is
    /// as we're time constrained here! In particular, I don't like how the fields differ depending on the type.
    /// Inheritance would leave us with a similar problem (check then cast), but it would be slightly safer.
    /// </remarks>
    public struct Holiday
    {
        /// <summary>
        /// Type of holiday
        /// </summary>
        public HolidayType Type { get; set; }

        /// <summary>
        /// If HolidayType is NOT ParticularDayOfMonth, then this is the date the holiday falls on -- ignore year!
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// If HolidayType is ParticularDayOfMonth, then this is the month that it applies
        /// </summary>
        public MonthOfYear ApplicableMonth { get; set; }
        
        /// <summary>
        /// If HolidayType is ParticularDayOfMonth, then this is the day that it applies
        /// </summary>
        public DayOfWeek ApplicableDay { get; set; }
        
        /// <summary>
        /// If HolidayType is ParticularDayOfMonth, then this is the occurence of that day that applies
        /// </summary>
        public int OccurenceInMonth { get; set; }
    }
}