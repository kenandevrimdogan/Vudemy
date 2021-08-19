using FreeCourse.Services.Discount.Dtos.Discounts;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<ResponseDTO<List<DiscountDTO>>> GetAllAsync();

        Task<ResponseDTO<DiscountDTO>> GetByIdAsync(int id);

        Task<ResponseDTO<NoContent>> SaveAsync(CreateDiscountDTO createDiscount);

        Task<ResponseDTO<NoContent>> UpdateAsync(UpdateDiscountDTO updateDiscount);

        Task<ResponseDTO<NoContent>> DeleteAsync(int id);

        Task<ResponseDTO<DiscountDTO>> GetByCodeAndUserIdAsync(string code, string userId);
    }
}
