using BusinessDaysBetween.Business.Services;
using BusinessDaysBetween.Business.Tests.TestHelpers;
using Xunit;

namespace BusinessDaysBetween.Business.Tests.Services
{
    public class BusinessDayCalculatorServiceTests
    {
        [Theory]
        [InlineData("2020-09-01", "2020-09-01", 0)]
        [InlineData("2020-09-01", "2020-09-02", 0)]
        [InlineData("2020-09-01", "2020-09-04", 2)] // within the same week, no weekends
        [InlineData("2020-09-01", "2020-09-08", 4)] // crosses single weekend, less than a full week
        [InlineData("2020-09-01", "2020-09-29", 19)] // crosses several weekends
        public void CalculateBusinessDaysBetween_ReturnsExpected(string startDateRaw, string endDateRaw,
            int expected)
        {
            var startDate = DateTimeHelpers.Iso8601ToDateTime(startDateRaw);
            var endDate = DateTimeHelpers.Iso8601ToDateTime(endDateRaw);
            var sut = new BusinessDayCalculatorService();

            var result = sut.CalculateBusinessDaysBetween(startDate, endDate);
            
            Assert.Equal(expected, result);
        }
    }
}