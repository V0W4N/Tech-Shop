using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Models;
using Tech_Shop.Services;

namespace Tech_Shop.Controllers
{
        public class WishlistController : Controller
        {
            private ApplicationDbContext _db = new ApplicationDbContext();
            private DeviceContext db = new DeviceContext();
            private CartService _cartService = new CartService();
            private OrderService _orderService = new OrderService();
            private WishlistService _wishlistService = new WishlistService();

        public WishlistController(CartService cartService, OrderService orderService)
            {
                _cartService = cartService;
                _orderService = orderService;
            }
            public WishlistController()
            {
            }
            public ActionResult Index()
            {
                var WL = _wishlistService.GetWishlistItems();
                return View(WL);
            }


        // POST: Products/AddToCart
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            if (device != null)
            {
                _cartService.AddToCart(id, device);
                _wishlistService.RemoveFromWishlist(id);
                TempData["AddedProductName"] = device.DeviceName;
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
            public ActionResult RemoveFromWishlist(int id)
            {
                _wishlistService.RemoveFromWishlist(id);
                return RedirectToAction("Index");
            }

        }
}
