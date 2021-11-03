using FreeCourse.Frontends.Web.Models.Baskets;
using FreeCourse.Frontends.Web.Models.Discounts;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Controllers
{
    [Authorize]
    public class BasketsController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketsController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            var basket = await _basketService.Get();
            return View(basket);
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseIdAsync(courseId);

            var basketItem = new BasketItemViewModel
            {
                CourseId = course.Id,
                CourseName = course.Name,
                Price = course.Price,
            };

            await _basketService.AddBasketItem(basketItem);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItem(courseId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput input)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).FirstOrDefault();
                return RedirectToAction(nameof(Index));
            }

            var discountStatus = await _basketService.ApplyDiscount(input.Code);

            TempData["discountStatus"] = discountStatus;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelApplyDiscount()
        {
            await _basketService.CancelAplyDiscount();
            return RedirectToAction(nameof(Index));
        }
    }
}
