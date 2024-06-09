using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tech_Shop.Models;
using Tech_Shop.Services;

namespace Tech_Shop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private CartService _cartService = new CartService();
        private OrderService _orderService = new OrderService();
        private WishlistService _wishlistService = new WishlistService();
        private List<string> roleList = new List<string> { "Moderator", "Admin", "PowerUser" };


        // GET: Products
        public ActionResult Index()
        {
            var products = _db.Products.ToList();
            var cartItems = _cartService.GetCartItems();
            var wlItems = _wishlistService.GetWishlistItems();

            var list = new ProductListWithQ
            {
                Products = products,
                CartItems = cartItems,
                WishItems = wlItems,
                IsAdmin = IsInRoleList(roleList, User)
            };

            string addedProductName = TempData["AddedProductName"] as string;
            TempData.Remove("AddedProductName");
            if (addedProductName != "")
            {
                ViewBag.AddedProductName = addedProductName;
            }

            return View(list);
        }


        // POST: Products/AddToCart
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (product != null)
            {
                _cartService.AddToCart(id, product);
                TempData["AddedProductName"] = product.Name;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddToWishlist(int id)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (product != null)
            {
                _wishlistService.AddToWishlist(id, product);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromWishlist(int id)
        {
            _wishlistService.RemoveFromWishlist(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Index");
        }
        // Other actions...

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Category,ProductId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [Authorize]
        public bool ConfirmPurchase()
        {
            var cartItems = _cartService.GetCartItems();

            if (!cartItems.Any())
            {
                return false; // No items to purchase
            }

            try
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderItems = cartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalAmount = ci.Product.Price
                    }).ToList()
                };

                _db.Orders.Add(order);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Category,ProductId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected bool IsInRoleList(List<string> roles, IPrincipal user)
        {
            foreach (string role in roleList)
            {
                if (user.IsInRole(role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
