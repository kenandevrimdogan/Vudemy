using FreeCourse.Frontends.Web.Models.Discounts;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Shared.Dtos.Response;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            var response = await _httpClient.GetAsync($"discounts/getByCodeAsync/{discountCode}");

            if(!response.IsSuccessStatusCode)
            {
                return null;
            }

            var discount = await response.Content.ReadFromJsonAsync<ResponseDTO<DiscountViewModel>>();

            return discount.Data;
        }
    }
}
