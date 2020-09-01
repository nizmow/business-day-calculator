using System.Threading;
using System.Threading.Tasks;
using BusinessDaysBetween.Business.Commands;
using BusinessDaysBetween.Business.Services;
using MediatR;
using MediatR.Pipeline;

namespace BusinessDaysBetween.Business.Application
{
    public class CalculateBusinessDayHandler : IRequestHandler<CalculateBusinessDayCommand, int>
    {
        private readonly IBusinessDayCalculatorService _businessDayCalculatorService;

        public CalculateBusinessDayHandler(IBusinessDayCalculatorService businessDayCalculatorService)
        {
            _businessDayCalculatorService = businessDayCalculatorService;
        }

        public Task<int> Handle(CalculateBusinessDayCommand request, CancellationToken cancellationToken)
        {
            var result = _businessDayCalculatorService.CalculateBusinessDaysBetween(request.StartDate, request.EndDate);
            return Task.FromResult(result);
        }
    }
}
