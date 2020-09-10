using System;
using MediatR;

namespace BusinessDaysBetween.Business.Commands
{
    public class CalculateBusinessDaysCommand : IRequest<int>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}