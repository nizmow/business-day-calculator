using System;
using System.Collections.Generic;
using BusinessDaysBetween.Business.ValueObjects;

namespace BusinessDaysBetween.Business.Services
{
    public interface IBusinessDayCalculatorService
    {
        int CalculateBusinessDaysBetween(DateTime startDate, DateTime endDate, IEnumerable<IHoliday> holidays = null);
    }
}