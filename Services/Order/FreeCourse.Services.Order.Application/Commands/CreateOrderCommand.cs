using FreeCourse.Services.Order.Application.Dtos.Orders;
using FreeCourse.Shared.Dtos.Response;
using MediatR;
using System.Collections.Generic;

namespace FreeCourse.Services.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<ResponseDTO<CreatedOrderDTO>>
    {
        public string BuyerId { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; }

        public AddressDTO Address { get; set; }
    }
}
