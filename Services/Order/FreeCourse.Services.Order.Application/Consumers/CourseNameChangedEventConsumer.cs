using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChanedEvent>
    {
        private readonly OrderDBContext _orderDBContext;

        public CourseNameChangedEventConsumer(OrderDBContext orderDBContext)
        {
            _orderDBContext = orderDBContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChanedEvent> context)
        {
            var orderItems = await _orderDBContext.OrderItems.Where(x => x.ProductId == context.Message.CourseId).ToListAsync();

            orderItems.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.UpdatedName, x.PictureURL, x.Price);
            });

            await _orderDBContext.SaveChangesAsync();
        }
    }
}
