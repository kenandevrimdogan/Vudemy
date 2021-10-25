using FreeCourse.Frontends.Web.Models.Baskets;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Shared.Dtos.Response;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItem)
        {
            var basket = await Get();

            if (basket != null)
            {
                var basketItemExsists = basket.BasketItems.FirstOrDefault(x => x.CourseId == basketItem.CourseId);

                if (basketItemExsists == null)
                {
                    basket.BasketItems.Add(basketItem);
                }
            }
            else
            {
                basket = new BasketViewModel
                {
                    BasketItems = new List<BasketItemViewModel>
                    {
                        basketItem
                    }
                };
            }

            await SaveOrUpdate(basket);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            await CancelAplyDiscount();

            var basket = await Get();

            if (basket == null || string.IsNullOrEmpty(basket.DiscountCode))
            {
                return false;
            }

            var discount = await _discountService.GetDiscount(discountCode);

            if (discount == null)
            {
                return false;
            }

            basket.DiscountRate = discount.Rate;
            basket.DiscountCode = discount.Code;
            return await SaveOrUpdate(basket);
        }

        public async Task<bool> CancelAplyDiscount()
        {
            var basket = await Get();

            if (basket == null || string.IsNullOrEmpty(basket.DiscountCode))
            {
                return false;
            }

            basket.DiscountCode = null;
            basket.DiscountRate = null;
            return await SaveOrUpdate(basket);
        }

        public async Task<bool> Delete()
        {
            var result = await _httpClient.DeleteAsync("baskets");
            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> Get()
        {
            var response = await _httpClient.GetAsync("baskets");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var basketViewModel = await response.Content.ReadFromJsonAsync<ResponseDTO<BasketViewModel>>();

            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await Get();

            if (basket == null)
            {
                return false;
            }

            var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);

            if (deleteBasketItem == null)
            {
                return false;
            }

            var deleteResult = basket.BasketItems.Remove(deleteBasketItem);

            if (!basket.BasketItems.Any())
            {
                basket.DiscountCode = null;
            }

            var saveOrUpdateResponse = await SaveOrUpdate(basket);

            return saveOrUpdateResponse;
        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);

            return response.IsSuccessStatusCode;
        }
    }
}
