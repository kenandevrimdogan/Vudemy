using FreeCourse.Services.Order.Application.Dtos.Orders;
using FreeCourse.Shared.Dtos.Response;
using MediatR;
using System.Collections.Generic;

namespace FreeCourse.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery: IRequest<ResponseDTO<List<OrderDTO>>>
    {
        public string UserId { get; set; }

    }
}
