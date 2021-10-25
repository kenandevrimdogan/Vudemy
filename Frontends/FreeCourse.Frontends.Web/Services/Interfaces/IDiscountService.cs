using FreeCourse.Frontends.Web.Models.Discounts;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
    }
}
