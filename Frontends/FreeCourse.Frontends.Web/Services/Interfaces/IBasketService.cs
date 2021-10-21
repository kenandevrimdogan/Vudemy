using FreeCourse.Frontends.Web.Models.Baskets;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Interfaces
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);

        Task<BasketViewModel> Get();

        Task<bool> Delete();

        Task AddBasketItem(BasketItemViewModel basketItem);

        Task<bool> RemoveBasketItem(string courseId);

        Task<bool> ApplyDiscount(string discountCode);

        Task<bool> CancelAplyDiscount();
    }
}
