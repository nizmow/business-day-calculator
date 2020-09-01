using System;

namespace BusinessDaysBetween.Business.Services
{
    public interface IBusinessDayCalculatorService
    {
        int CalculateBusinessDaysBetween(DateTime startDate, DateTime endDate);
    }
}