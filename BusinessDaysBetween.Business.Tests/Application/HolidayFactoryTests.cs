using System;
using BusinessDaysBetween.Business.Application;
using BusinessDaysBetween.Business.Infrastructure;
using BusinessDaysBetween.Business.ValueObjects;
using Xunit;

namespace BusinessDaysBetween.Business.Tests.Application
{
    public class HolidayFactoryTests
    {
        [Fact]
        public void HolidayFactory_ReturnsCorrectObject_WithFixedHolidayDto()
        {
            // arrange
            var dto = new HolidayDto
            {
                Type = HolidayType.Fixed,
                Date = new DateTime(2020, 5, 5),
            };
            var sut = new HolidayFactory();
            
            // act
            var result = sut.CreateHoliday(dto) ;
            
            // assert
            Assert.IsType<FixedHoliday>(result);
            var typedResult = result as FixedHoliday;
            Assert.Equal(new DateTime(2020, 5, 5), typedResult.Date);
            Assert.False(typedResult.RollsToNextMonday);
        } 
        
        [Fact]
        public void HolidayFactory_ReturnsCorrectObject_WithRollsToMondayHolidayDto()
        {
            // arrange
            var dto = new HolidayDto
            {
                Type = HolidayType.RollsToMonday,
                Date = new DateTime(2020, 5, 5),
            };
            var sut = new HolidayFactory();
            
            // act
            var result = sut.CreateHoliday(dto) ;
            
            // assert
            Assert.IsType<FixedHoliday>(result);
            var typedResult = result as FixedHoliday;
            Assert.Equal(new DateTime(2020, 5, 5), typedResult.Date);
            Assert.True(typedResult.RollsToNextMonday);
        } 
        
        [Fact]
        public void HolidayFactory_ReturnsCorrectObject_WithParticularDayOfMonthHolidayDto()
        {
            // arrange
            var dto = new HolidayDto
            {
                Type = HolidayType.ParticularDayOfMonth,
                ApplicableDay = DayOfWeek.Monday,
                ApplicableMonth = MonthOfYear.October,
                OccurenceInMonth = 3,
            };
            var sut = new HolidayFactory();
            
            // act
            var result = sut.CreateHoliday(dto) ;
            
            // assert
            Assert.IsType<DayOfMonthHoliday>(result);
            var typedResult = result as DayOfMonthHoliday;
            Assert.Equal(3, typedResult.OccurenceInMonth);
            Assert.Equal(DayOfWeek.Monday, typedResult.ApplicableDay);
            Assert.Equal(MonthOfYear.October, typedResult.ApplicableMonth);
        } 
    }
}