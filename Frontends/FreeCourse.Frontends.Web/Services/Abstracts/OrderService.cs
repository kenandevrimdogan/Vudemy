using FreeCourse.Frontends.Web.Models.FaketPayments;
using FreeCourse.Frontends.Web.Models.Orders;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Shared.Dtos.Response;
using FreeCourse.Shared.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            var paymentInfoInput = new PaymentInfoInput
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice
            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderCreatedViewModel { IsSuccess = false, ErrorMessage = "Ödeme alınamadı" };
            }

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressViewModel
                {
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Province = checkoutInfoInput.Province,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                },
                OrderItems = basket.BasketItems.Select(x => new OrderItemCreateInput
                {
                    PictureURL = string.Empty,
                    Price = x.Price,
                    ProductId = x.CourseId,
                    ProductName = x.CourseName,
                }).ToList()
            };

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel { IsSuccess = false, ErrorMessage = "Sipariş oluşturulamadı" };
            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<OrderCreatedViewModel>();

            return orderCreatedViewModel;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDTO<List<OrderViewModel>>>("orders");

            return response.Data;
        }

        public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            throw new System.NotImplementedException();
        }
    }
}
