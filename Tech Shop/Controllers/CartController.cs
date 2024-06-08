using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech_Shop.Models;
using Tech_Shop.Services;

namespace Tech_Shop.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public CartController()
        {
        }
        public CartController(CartService cartService, OrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        public ActionResult Index()
        {
            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public ActionResult UpdateCart(int productId, int quantity)
        {
            _cartService.UpdateCart(productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteFromCart(int productId)
        {
            _cartService.DeleteFromCart(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ConfirmPurchase()
        {
            var result = _orderService.ConfirmPurchase();
            if (result)
            {
                // Clear the cart after successful purchase
                _cartService.EmptyCart();
                return RedirectToAction("PurchaseSuccess");
            }
            else
            {
                return RedirectToAction("PurchaseFailed");
            }
        }

        public ActionResult PurchaseSuccess()
        {
            return View();
        }

        public ActionResult PurchaseFailed()
        {
            return View();
        }
    }
}