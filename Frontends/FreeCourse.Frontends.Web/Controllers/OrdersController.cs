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

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            var orderStatus = await _orderservice.CreateOrder(checkoutInfoInput);

            if (!orderStatus.IsSuccess)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.ErrorMessage;
                return View();
            }

            return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderStatus.OrderId });
        }

        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        } 
    }
}
