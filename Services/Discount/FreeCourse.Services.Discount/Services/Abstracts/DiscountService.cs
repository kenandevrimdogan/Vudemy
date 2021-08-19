using FreeCourse.Services.Discount.Dtos.Discounts;
using FreeCourse.Services.Discount.Services.Interfaces;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services.Abstracts
{
    public class DiscountService : IDiscountService
    {
        public Task<ResponseDTO<NoContent>> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseDTO<List<DiscountDTO>>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseDTO<DiscountDTO>> GetByCodeAndUserId(string code, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseDTO<DiscountDTO>> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseDTO<NoContent>> Save(CreateDiscountDTO createDiscount)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseDTO<NoContent>> Update(UpdateDiscountDTO updateDiscount)
        {
            throw new System.NotImplementedException();
        }
    }
}
