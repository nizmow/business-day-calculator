using System;
using MediatR;

namespace BusinessDaysBetween.Business.Commands
{
    public class CalculateBusinessDayCommand : IRequest<int>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}