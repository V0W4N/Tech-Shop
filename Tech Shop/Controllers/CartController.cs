using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Models;
using Tech_Shop.Services;

namespace Tech_Shop.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private DeviceContext db = new DeviceContext();
        private CartService _cartService = new CartService();
        private OrderService _orderService = new OrderService();

        public CartController(CartService cartService, OrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }
        public CartController()
        {
        }
        public ActionResult Index()
        {
            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public ActionResult UpdateCart(int deviceId, int quantity)
        {
            _cartService.UpdateCart(deviceId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int deviceId)
        {
            _cartService.RemoveFromCart(deviceId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteFromCart(int deviceId)
        {
            _cartService.DeleteFromCart(deviceId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        
        public ActionResult ConfirmPurchase()
        {
            var result = _orderService.ConfirmPurchase();
            if (result)
            {
               if (User.Identity.IsAuthenticated)
                  {
                        var userId = User.Identity.GetUserId();
                        var cartItems = _cartService.GetCartItems().ToList();
                        var order = new Order
                        {
                            UserId = userId,
                            OrderDate = DateTime.Now,
                            TotalAmount = cartItems.Sum(item => item.Device.Price * item.Quantity),
                            OrderItems = cartItems.Select(item => new OrderItem
                            {
                                DeviceId = item.DeviceId,
                                Quantity = item.Quantity
                            }).ToList()
                        };
                        _orderService.AddOrder(order);
                }
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