using System;
using BusinessDaysBetween.Business.ValueObjects;
using Xunit;

namespace BusinessDaysBetween.Business.Tests.ValueObjects
{
    public class DayOfMonthHolidayTests
    {
        [Fact]
        public void GetDateForYear_ReturnsExpectedValue_WhenDayIsPresent()
        {
            // arrange
            var sut = new DayOfMonthHoliday(
                MonthOfYear.September, 
                DayOfWeek.Tuesday, 
                2);
            
            // act
            var result = sut.GetDateForYear(2020);
            
            // assert
            Assert.True(result.present);
            Assert.Equal(new DateTime(2020, 9, 8), result.date);
        }

        [Theory]
        [InlineData(2019, false)]
        [InlineData(2020, true)]
        public void GetDateForYear_ReturnsNotPresent_WhenDayIsNotPresent(int year, bool expected)
        {
            // arrange
            var sut = new DayOfMonthHoliday(
                MonthOfYear.February, 
                DayOfWeek.Saturday, 
                5);
            
            // act
            var result = sut.GetDateForYear(year);
            
            // assert
            Assert.Equal(expected, result.present);
        }

        [Fact]
        public void GetDateForYear_ReturnsNotPresent_ForOccurenceOver5()
        {
            
            // arrange
            var sut = new DayOfMonthHoliday(
                MonthOfYear.January, 
                DayOfWeek.Wednesday, 
                6);
            
            // act
            var result = sut.GetDateForYear(2020);
            
            // assert
            Assert.False(result.present);
        }
    }
}