using System;
using BusinessDaysBetween.Business.ValueObjects;

namespace BusinessDaysBetween.Business.Infrastructure
{
    /// <summary>
    /// Raw DTO for pulling data out of our JSON configuration.
    /// </summary>
    public struct HolidayDto
    {
        /// <summary>
        /// Type of holiday
        /// </summary>
        public HolidayType? Type { get; set; }

        /// <summary>
        /// If HolidayType is NOT ParticularDayOfMonth, then this is the date the holiday falls on -- ignore year!
        /// </summary>
        public DateTime? Date { get; set; }
        
        /// <summary>
        /// If HolidayType is ParticularDayOfMonth, then this is the month that it applies
        /// </summary>
        public MonthOfYear? ApplicableMonth { get; set; }
        
        /// <summary>
        /// If HolidayType is ParticularDayOfMonth, then this is the day that it applies
        /// </summary>
        public DayOfWeek? ApplicableDay { get; set; }
        
        /// <summary>
        /// If HolidayType is ParticularDayOfMonth, then this is the occurence of that day that applies
        /// </summary>
        public int? OccurenceInMonth { get; set; }
    }
}