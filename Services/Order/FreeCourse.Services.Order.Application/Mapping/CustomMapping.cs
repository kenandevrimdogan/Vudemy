using AutoMapper;
using FreeCourse.Services.Order.Application.Dtos.Orders;

namespace FreeCourse.Services.Order.Application.Mapping
{
    public class CustomMapping: Profile
    {
        public CustomMapping()
        {
            CreateMap<Domain.OrderAggregate.Order, OrderDTO>().ReverseMap();
            CreateMap<Domain.OrderAggregate.OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Domain.OrderAggregate.Address, AddressDTO>().ReverseMap();
        }
    }
}
