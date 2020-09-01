using System.Threading.Tasks;
using BusinessDaysBetween.Api.ViewModels;
using BusinessDaysBetween.Business.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BusinessDaysBetween.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessDaysCalculatorController
    {
        private readonly IMediator _mediator;

        public BusinessDaysCalculatorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<ActionResult<CalculateBusinessDaysBetweenResponse>> CalculateBusinessDaysBetween(
            [FromBody] CalculateBusinessDaysBetweenRequest request)
        {
            var days = await _mediator.Send(new CalculateBusinessDayCommand
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            });
            return new CalculateBusinessDaysBetweenResponse {Days = days};
        }
    }
}