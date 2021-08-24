using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Dtos.Orders;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Dtos.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseDTO<CreatedOrderDTO>>
    {
        private readonly OrderDBContext _orderDBContext;

        public CreateOrderCommandHandler(OrderDBContext orderDBContext)
        {
            _orderDBContext = orderDBContext;
        }

        public async Task<ResponseDTO<CreatedOrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

            var newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

            request.OrderItems.ForEach(item =>
            {
                newOrder.AddOrderItem(item.ProductId, item.ProductName, item.PictureURL, item.Price);
            });

            await _orderDBContext.Orders.AddAsync(newOrder);

            var result = await _orderDBContext.SaveChangesAsync();

            return ResponseDTO<CreatedOrderDTO>.Success(new CreatedOrderDTO { OrderId = newOrder.Id }, HttpStatusCode.OK);
        }
    }
}
