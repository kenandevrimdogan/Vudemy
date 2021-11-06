using FreeCourse.Services.Basket.Services.Interfaces;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChanedEvent>
    {
        private readonly IBasketService _basketService;

        public CourseNameChangedEventConsumer(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
        }

        public async Task Consume(ConsumeContext<CourseNameChanedEvent> context)
        {
            var basketResponse = await _basketService.GetBaskets();

            if(!basketResponse.IsSuccessful)
            {
                return;
            }

            foreach (var baskets in basketResponse.Data)
            {
                if(baskets.BasketItems != null && baskets.BasketItems.Any())
                {
                    foreach (var item in baskets.BasketItems.Where(x=> x.CourseId == context.Message.CourseId))
                    {
                        item.CourseName = context.Message.UpdatedName;
                    }

                    await _basketService.SaveOrUpdate(baskets);
                }
            }
        }
    }
}
