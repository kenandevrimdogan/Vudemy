using FreeCourse.Frontends.Web.Models.Baskets;
using FreeCourse.Frontends.Web.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task AddBasketItem(BasketItemViewModel basketItem)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ApplyDiscount(string discountCode)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CancelAplyDiscount()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete()
        {
            throw new System.NotImplementedException();
        }

        public Task<BasketViewModel> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveBasketItem(string courseId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
