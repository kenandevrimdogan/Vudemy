using FreeCourse.Services.FakePayment.Models.FaketPayments;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        [HttpPost]
        public IActionResult RecivePayment(PaymentDTO payment)
        {
            return CreateActionResultInstance<NoContent>(ResponseDTO<NoContent>.Success(HttpStatusCode.OK)); 
        }
    }
}
