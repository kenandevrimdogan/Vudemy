using FreeCourse.Frontends.Web.Models.FaketPayments;
using FreeCourse.Frontends.Web.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class PaymentService : IPaymentService
    {
        public readonly HttpClient _httpClient;

        public async Task<bool> ReceivePayment(PaymentInfoInput paymentInfo)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInfoInput>("fakepayments", paymentInfo);

            return response.IsSuccessStatusCode;
        }
    }
}
