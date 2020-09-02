using System.Threading;
using System.Threading.Tasks;
using BusinessDaysBetween.Business.Commands;
using BusinessDaysBetween.Business.Infrastructure;
using BusinessDaysBetween.Business.Services;
using MediatR;

namespace BusinessDaysBetween.Business.Application
{
    public class CalculateBusinessDayHandler : IRequestHandler<CalculateBusinessDayCommand, int>
    {
        private readonly IBusinessDayCalculatorService _businessDayCalculatorService;
        private readonly IHolidayRepository _holidayRepository;

        public CalculateBusinessDayHandler(IBusinessDayCalculatorService businessDayCalculatorService, IHolidayRepository holidayRepository)
        {
            _businessDayCalculatorService = businessDayCalculatorService;
            _holidayRepository = holidayRepository;
        }

        public async Task<int> Handle(CalculateBusinessDayCommand request, CancellationToken cancellationToken)
        {
            var holidays = await _holidayRepository.LoadHolidays();
            var result = _businessDayCalculatorService.CalculateBusinessDaysBetween(request.StartDate, request.EndDate, holidays);
            return result;
        }
    }
}
