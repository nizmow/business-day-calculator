using System;

namespace BusinessDaysBetween.Api.ViewModels
{
    public class CalculateBusinessDaysBetweenRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}