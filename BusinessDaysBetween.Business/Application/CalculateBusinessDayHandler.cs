using System.Linq;
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
        private readonly IHolidayFactory _holidayFactory;

        public CalculateBusinessDayHandler(
            IBusinessDayCalculatorService businessDayCalculatorService,
            IHolidayRepository holidayRepository,
            IHolidayFactory holidayFactory)
        {
            _businessDayCalculatorService = businessDayCalculatorService;
            _holidayRepository = holidayRepository;
            _holidayFactory = holidayFactory;
        }

        public async Task<int> Handle(CalculateBusinessDayCommand request, CancellationToken cancellationToken)
        {
            var holidayDtos = await _holidayRepository.LoadHolidays();
            var holidays = holidayDtos.Select(_holidayFactory.CreateHoliday);
            var result = _businessDayCalculatorService.CalculateBusinessDaysBetween(request.StartDate, request.EndDate, holidays);
            return result;
        }
    }
}
