using AutoMapper;
using FreeCourse.Services.Discount.Dtos.Discounts;

namespace FreeCourse.Services.Discount.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Models.Discounts.Discount, DiscountDTO>().ReverseMap();

            CreateMap<Models.Discounts.Discount, CreateDiscountDTO>().ReverseMap();
            CreateMap<Models.Discounts.Discount, UpdateDiscountDTO>().ReverseMap();

        }
    }
}
