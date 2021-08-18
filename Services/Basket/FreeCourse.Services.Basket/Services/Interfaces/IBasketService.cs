using FreeCourse.Services.Basket.Dtos.Baskets;
using FreeCourse.Shared.Dtos.Response;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services.Interfaces
{
    public interface IBasketService
    {
        Task<ResponseDTO<BasketDTO>> GetBasket(string userId);

        Task<ResponseDTO<bool>> SaveOrUpdate(BasketDTO basket);

        Task<ResponseDTO<bool>> Delete(string userId);
    }
}
