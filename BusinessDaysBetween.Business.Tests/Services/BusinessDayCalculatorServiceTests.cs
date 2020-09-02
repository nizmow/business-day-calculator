using System;
using System.Collections.Generic;
using BusinessDaysBetween.Business.Services;
using BusinessDaysBetween.Business.Tests.TestHelpers;
using BusinessDaysBetween.Business.ValueObjects;
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
        [InlineData("2020-03-20", "2020-03-25", 2)] // will later be a holiday test
        public void CalculateBusinessDaysBetween_ReturnsExpected(string startDateRaw, string endDateRaw,
            int expected)
        {
            var startDate = DateTimeHelpers.Iso8601ToDateTime(startDateRaw);
            var endDate = DateTimeHelpers.Iso8601ToDateTime(endDateRaw);
            var sut = new BusinessDayCalculatorService();

            var result = sut.CalculateBusinessDaysBetween(startDate, endDate);
            
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2020-03-20", "2020-03-25", 1)] // same as above, different result
        [InlineData("2020-03-20", "2020-03-28", 3)]
        [InlineData("2020-03-16", "2020-03-24", 3)]
        public void CalculateBusinessDaysBetween_ReturnsExpected_WithProvidedHolidays(string startDateRaw, string endDateRaw,
            int expected)
        {
            // arrange
            var holidays = new List<Holiday>
            {
                new Holiday
                {
                    Type = HolidayType.RollsToMonday,
                    Date = new DateTime(2019, 3, 22) // this is a Sunday in 2020!
                },
                new Holiday
                {
                    Type = HolidayType.Fixed,
                    Date = new DateTime(1900, 3, 25)
                },
                new Holiday
                {
                    Type = HolidayType.ParticularDayOfMonth,
                    ApplicableDay = DayOfWeek.Wednesday,
                    ApplicableMonth = MonthOfYear.March,
                    OccurenceInMonth = 3,
                }
            };
            var startDate = DateTimeHelpers.Iso8601ToDateTime(startDateRaw);
            var endDate = DateTimeHelpers.Iso8601ToDateTime(endDateRaw);
            var sut = new BusinessDayCalculatorService();
            
            // act
            var result = sut.CalculateBusinessDaysBetween(startDate, endDate, holidays);

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetHolidayDateForYear_ReturnsExpectedDate_ForHolidayTypeFixed()
        {
            // arrange
            var holiday = new Holiday
            {
                Type = HolidayType.Fixed,
                Date = new DateTime(1900, 5, 5)
            };
            var sut = new BusinessDayCalculatorService();
            
            // act
            var result = sut.GetHolidayDateForYear(holiday, 2020);
            
            // assert
            Assert.True(result.present);
            Assert.Equal(new DateTime(2020, 5, 5), result.date);
        }

        [Fact]
        public void GetHolidayDateForYear_ReturnsExpectedDate_ForHolidayTypeRollsToMonday()
        {
            
            // arrange
            var holiday = new Holiday
            {
                Type = HolidayType.RollsToMonday,
                Date = new DateTime(2019, 3, 22) // this is a Sunday in 2020!
            };
            var sut = new BusinessDayCalculatorService();
            
            // act
            var result = sut.GetHolidayDateForYear(holiday, 2020);
            
            // assert
            Assert.True(result.present);
            Assert.Equal(new DateTime(2020, 3, 23), result.date);
        }
        
        [Fact]
        public void GetHolidayDateForYear_ReturnsExpectedDate_ForHolidayTypeParticularDayOfMonth()
        {
            // arrange
            var holiday = new Holiday
            {
                Type = HolidayType.ParticularDayOfMonth,
                ApplicableDay = DayOfWeek.Tuesday,
                ApplicableMonth = MonthOfYear.September,
                OccurenceInMonth = 2,
            };
            var sut = new BusinessDayCalculatorService();
            
            // act
            var result = sut.GetHolidayDateForYear(holiday, 2020);
            
            // assert
            Assert.True(result.present);
            Assert.Equal(new DateTime(2020, 9, 8), result.date);
        }

        [Fact]
        public void GetHolidayDateForYear_ReturnsNotPresent_ForHolidayTypeParticularDayOfMonth_OnYearItDoesntOccur()
        {
            // arrange
            var holiday = new Holiday
            {
                Type = HolidayType.ParticularDayOfMonth,
                ApplicableDay = DayOfWeek.Friday,
                ApplicableMonth = MonthOfYear.February,
                OccurenceInMonth = 5,
            };
            var sut = new BusinessDayCalculatorService();
            
            // act
            var result = sut.GetHolidayDateForYear(holiday, 2020);
            
            // assert
            Assert.False(result.present);
        }
        
        [Fact]
        public void GetHolidayDateForYear_ReturnsNotPresent_ForOccurenceOver5()
        {
            // arrange
            var holiday = new Holiday
            {
                Type = HolidayType.ParticularDayOfMonth,
                ApplicableDay = DayOfWeek.Friday,
                ApplicableMonth = MonthOfYear.January,
                OccurenceInMonth = 6,
            };
            var sut = new BusinessDayCalculatorService();
            
            // act
            var result = sut.GetHolidayDateForYear(holiday, 2020);
            
            // assert
            Assert.False(result.present);
        }
    }
}