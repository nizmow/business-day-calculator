using BusinessDaysBetween.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusinessDaysBetween.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessDaysCalculatorController
    {
        [HttpPost]
        public ActionResult<CalculateBusinessDaysBetweenResponse> CalculateBusinessDaysBetween(
            [FromBody] CalculateBusinessDaysBetweenRequest request)
        {
            return new CalculateBusinessDaysBetweenResponse {Days = 5};
        }
    }
}