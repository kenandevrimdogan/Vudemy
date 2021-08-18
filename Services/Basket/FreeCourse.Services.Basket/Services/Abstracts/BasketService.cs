using FreeCourse.Services.Basket.Dtos.Baskets;
using FreeCourse.Services.Basket.Services.Interfaces;
using FreeCourse.Shared.Dtos.Response;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services.Abstracts
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<ResponseDTO<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDB().KeyDeleteAsync(userId);

            return status ? ResponseDTO<bool>.Success(HttpStatusCode.OK) : ResponseDTO<bool>.Fail("Basket not found", HttpStatusCode.NotFound);
        }

        public async Task<ResponseDTO<BasketDTO>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDB().StringGetAsync(userId);

            if (string.IsNullOrEmpty(existBasket))
            {
                return ResponseDTO<BasketDTO>.Fail("Basket not found", HttpStatusCode.BadRequest);
            }

            return ResponseDTO<BasketDTO>.Success(JsonSerializer.Deserialize<BasketDTO>(existBasket), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<bool>> SaveOrUpdate(BasketDTO basket)
        {
            var status = await _redisService.GetDB().StringSetAsync(basket.UserId, JsonSerializer.Serialize(basket));

            return status ? ResponseDTO<bool>.Success(HttpStatusCode.OK) : ResponseDTO<bool>.Fail("Basket could not update or save", HttpStatusCode.InternalServerError);
        }
    }
}
