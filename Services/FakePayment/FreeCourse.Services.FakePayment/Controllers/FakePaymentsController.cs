using FreeCourse.Services.FakePayment.Models.FaketPayments;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> RecivePayment(PaymentDTO payment)
        {
            var sendEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand
            {
                BuyerId = payment.Order.BuyerId,
                Province = payment.Order.Address.Province,
                District = payment.Order.Address.District,
                Street = payment.Order.Address.Street,
                Line = payment.Order.Address.Line,
                ZipCode = payment.Order.Address.ZipCode,
                OrderItems = payment.Order.OrderItems.Select(x => new OrderItem
                {
                    PictureURL = x.PictureURL,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                }).ToList()
            };


            try
            {
                await sendEndPoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
            }
            catch (Exception ex)
            {

                throw;
            }

            return CreateActionResultInstance<NoContent>(ResponseDTO<NoContent>.Success(HttpStatusCode.OK));
        }
    }
}
