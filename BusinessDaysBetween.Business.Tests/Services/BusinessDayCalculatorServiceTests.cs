﻿using System;
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
        [InlineData("2020-03-16", "2020-03-24", 5)]
        [InlineData("2022-03-14", "2029-03-15", 1827)]
        [InlineData("2020-01-29", "2020-01-31", 1)]
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
            var holidays = new List<IHoliday>
            {
                new FixedHoliday(false, new DateTime(1900, 3, 25)),
                new FixedHoliday(true, new DateTime(2019, 3, 22)),
                new DayOfMonthHoliday(MonthOfYear.March, DayOfWeek.Wednesday, 3),
            };
            var startDate = DateTimeHelpers.Iso8601ToDateTime(startDateRaw);
            var endDate = DateTimeHelpers.Iso8601ToDateTime(endDateRaw);
            var sut = new BusinessDayCalculatorService();
            
            // act
            var result = sut.CalculateBusinessDaysBetween(startDate, endDate, holidays);

            // assert
            Assert.Equal(expected, result);
        }
    }
}