using FreeCourse.Frontends.Web.Models.Orders;
using FreeCourse.Frontends.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderservice;

        public OrdersController(IBasketService basketService, IOrderService orderservice)
        {
            _basketService = basketService;
            _orderservice = orderservice;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;

            return View(new CheckoutInfoInput());
        }
    }
}
