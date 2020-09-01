using System;

namespace BusinessDaysBetween.Business.Tests.TestHelpers
{
    /// <summary>
    /// Helpers useful for tests for dealing with dates
    /// </summary>
    public static class DateTimeHelpers
    {
        /// <summary>
        /// Converts a standard ISO 8601 date string to a .NET DateTime object
        /// </summary>
        /// <param name="isoDateTime">Standard ISO 8601 date string</param>
        /// <returns>Equivalent .NET DateTime object</returns>
        /// <remarks>Lets easily use xunit theories with dates</remarks>
        public static DateTime Iso8601ToDateTime(string isoDateTime) => DateTime.Parse(isoDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }
}