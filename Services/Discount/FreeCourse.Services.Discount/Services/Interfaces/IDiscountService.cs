using FreeCourse.Services.Discount.Dtos.Discounts;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<ResponseDTO<List<DiscountDTO>>> GetAll();

        Task<ResponseDTO<DiscountDTO>> GetById(int id);

        Task<ResponseDTO<NoContent>> Save(CreateDiscountDTO createDiscount);

        Task<ResponseDTO<NoContent>> Update(UpdateDiscountDTO updateDiscount);

        Task<ResponseDTO<NoContent>> Delete(int id);

        Task<ResponseDTO<DiscountDTO>> GetByCodeAndUserId(string code, string userId);
    }
}
