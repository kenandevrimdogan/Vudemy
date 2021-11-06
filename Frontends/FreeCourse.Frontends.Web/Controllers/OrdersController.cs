using FreeCourse.Frontends.Web.Models.Orders;
using FreeCourse.Frontends.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
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
            //var orderStatus = await _orderservice.CreateOrder(checkoutInfoInput);

            var orderSuspend = await _orderservice.SuspendOrder(checkoutInfoInput);
            if (!orderSuspend.IsSuccess)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderSuspend.ErrorMessage;
                return View();
            }

            //return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderStatus.OrderId });
            return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderservice.GetOrder());
        }
    }
}
