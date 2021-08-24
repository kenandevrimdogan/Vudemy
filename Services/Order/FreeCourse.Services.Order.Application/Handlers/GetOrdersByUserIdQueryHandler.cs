using FreeCourse.Services.Order.Application.Dtos.Orders;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Dtos.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, ResponseDTO<List<OrderDTO>>>
    {
        private readonly OrderDBContext _orderDbContext;

        public GetOrdersByUserIdQueryHandler(OrderDBContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<ResponseDTO<List<OrderDTO>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderDbContext.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return ResponseDTO<List<OrderDTO>>.Success(new List<OrderDTO>(), HttpStatusCode.OK);
            }

            var orderDTO = ObjectMapper.Mapper.Map<List<OrderDTO>>(orders);

            return ResponseDTO<List<OrderDTO>>.Success(orderDTO, HttpStatusCode.OK);
        }
    }
}
