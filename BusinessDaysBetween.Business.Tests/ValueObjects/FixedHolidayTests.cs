using System;
using BusinessDaysBetween.Business.ValueObjects;
using Xunit;

namespace BusinessDaysBetween.Business.Tests.ValueObjects
{
    public class FixedHolidayTests
    {
        [Fact]
        public void GetDateForYear_ReturnsExpectedValue_WhenDoesntRollToMonday()
        {
            // arrange
            var sut = new FixedHoliday(false, new DateTime(1900, 5, 5));
            
            // act
            var result = sut.GetDateForYear(2020);
            
            // assert
            Assert.True(result.present);
            Assert.Equal(new DateTime(2020, 5, 5), result.date);
        }

        [Fact]
        public void GetDateForYear_ReturnsExpectedValue_WhenRollsToMonda()
        {
            // arrange
            var sut = new FixedHoliday(true, new DateTime(2019, 3, 22));
            
            // act
            var result = sut.GetDateForYear(2020);
            
            // assert
            Assert.True(result.present);
            Assert.Equal(new DateTime(2020, 3, 23), result.date);
        }
    }
}