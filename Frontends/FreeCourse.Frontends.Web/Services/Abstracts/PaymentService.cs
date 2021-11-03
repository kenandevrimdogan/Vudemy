using FreeCourse.Frontends.Web.Models.FaketPayments;
using FreeCourse.Frontends.Web.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class PaymentService : IPaymentService
    {
        public readonly HttpClient _httpClient;

        public Task<bool> ReceivePayment(PaymentInfoInput paymentInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
