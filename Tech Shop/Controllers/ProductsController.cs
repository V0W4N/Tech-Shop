using Microsoft.AspNet.Identity;
using Tech_Shop.Models;
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
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Services;
using Tech_Shop.ViewModels;
using System.Web.WebPages;
using System.Web.Services.Description;

namespace Tech_Shop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private DeviceContext db = new DeviceContext();
        private CartService _cartService = new CartService();
        private OrderService _orderService = new OrderService();
        private WishlistService _wishlistService = new WishlistService();
        private List<string> roleList = new List<string> { "Moderator", "Admin", "PowerUser" };


        // GET: Products
        public ActionResult Index()
        {
            var devices = db.Devices.Include(d => d.Category).Include(d => d.AttributeValues).ToList();
            var cartItems = _cartService.GetCartItems();
            var wlItems = _wishlistService.GetWishlistItems();

            var list = new ProductListWithQ
            {
                Devices = devices,
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
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.DeviceCategories, "CategoryId", "CategoryName");

            DeviceCategoryView dcv = new DeviceCategoryView { };
            return View(dcv);
            
        }
        [HttpPost]
        public ActionResult Create(DeviceCategoryView dcv)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("CreateWithCategory", new { id = dcv.CategoryId });
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult CreateWithCategory(int? id)
        {

            if (id == null) return HttpNotFound();
    
            var device = new DeviceEditViewModel();
            var attributes = db.DeviceCategoryAttributes
                .Where(attr => attr.CategoryId == id)
                .ToList();
            List<AttributeViewModel> attributesModel = new List<AttributeViewModel>();
            foreach (var attribute in attributes)
            {

                DeviceCategoryAttributeValue val = new DeviceCategoryAttributeValue { Attribute = attribute, AttributeId = attribute.AttributeId };
                
                 attributesModel.Add(new AttributeViewModel()
                {
                    AttributeId = attribute.AttributeId,
                    AttributeName = attribute.AttributeName,
                    AttributeValue = val
                });
            }
            var model = new DeviceEditViewModel
            {
                DeviceId = device.DeviceId,
                DeviceName = device.DeviceName,
                Manufacturer = device.Manufacturer,
                Description = device.Description,
                Price = device.Price,
                Device = device.Device,
                CategoryId = db.DeviceCategories.Find(id).CategoryId,
                CategoryName = db.DeviceCategories.Find(id).CategoryName,
                DeviceImage = device.DeviceImage,
                Attributes = attributesModel,
            };
            

    

            return View(model);
        }

     



        // POST: Device/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithCategory(DeviceEditViewModel deviceViewModel)
        {
            
                List<DeviceCategoryAttributeValue> attributeValues = new List<DeviceCategoryAttributeValue>();
                foreach(var item in db.DeviceCategoryAttributes
                                    .Where(a => a.CategoryId == deviceViewModel.CategoryId)
                                        .ToList())
                {

                    attributeValues.Add(new DeviceCategoryAttributeValue
                    {
                        AttributeId = item.AttributeId,
                        Value = item.AttributeValues.FirstOrDefault(x => x.AttributeId == item.AttributeId)?.Value,
                    });
                }
                Device device = new Device
                {
                    DeviceName = deviceViewModel.DeviceName,
                    Manufacturer = deviceViewModel.Manufacturer,
                    Description = deviceViewModel.Description,
                    Price = deviceViewModel.Price,
                    CategoryId = deviceViewModel.CategoryId,
                    DeviceImage = deviceViewModel.DeviceImage,
                    AttributeValues = attributeValues
                };

                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("");
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            var device = db.Devices.Find(id);
            if (device == null) return HttpNotFound();
            List<AttributeViewModel> attributesModel = new List<AttributeViewModel>();
            var attributes = db.DeviceCategoryAttributes
                .Where(attr => attr.CategoryId == device.CategoryId)
                .ToList();
            foreach (var attribute in attributes)
            {
                DeviceCategoryAttributeValue val = new DeviceCategoryAttributeValue { Attribute = attribute, AttributeId = attribute.AttributeId};
                var attributeValue = device.AttributeValues.FirstOrDefault(a => a.AttributeId == attribute.AttributeId);
                if (attributeValue != null) val = attributeValue;
                attributesModel.Add(new AttributeViewModel()
                {
                    AttributeId = attribute.AttributeId,
                    AttributeName = attribute.AttributeName,
                    AttributeValue = val
                });
            }
            var model = new DeviceEditViewModel
            {
                DeviceId = device.DeviceId,
                DeviceName = device.DeviceName,
                Manufacturer = device.Manufacturer,
                Description = device.Description,
                Price = device.Price,
                Device = device,
                CategoryId = device.CategoryId,
                CategoryName = device.Category.CategoryName,
                DeviceImage = device.DeviceImage,
                Attributes = attributesModel,
            };
            ViewBag.Categories = new SelectList(db.DeviceCategories, "CategoryId", "CategoryName", model.CategoryId);
            var categories = db.DeviceCategories.ToList();
            var categoryItems = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            });
            ViewBag.CategoryItems = categoryItems;
            return View(model);
        }

        // POST: Devices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceEditViewModel model)
        {
            
                var device = db.Devices.Find(model.DeviceId);
                if (device == null) return HttpNotFound();
                device.DeviceName = model.DeviceName;
                device.Manufacturer = model.Manufacturer;
                device.Description = model.Description;
                device.Price = model.Price;
                device.CategoryId = model.CategoryId;
                device.DeviceImage = model.DeviceImage;
                if (model.Attributes != null)
                {
                    device.AttributeValues = model.Attributes.Select(attr => attr.AttributeValue).ToList();
                }
                db.SaveChanges();
                return RedirectToAction("Index");
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var device = db.Devices.Find(id);
            if (device == null) return HttpNotFound();
            var model = new DeviceViewModel
            {
                DeviceId = device.DeviceId,
                DeviceName = device.DeviceName,
                Manufacturer = device.Manufacturer,
                Description = device.Description,
                Price = device.Price,
                CategoryId = device.CategoryId,
                CategoryName = device.Category.CategoryName,
                AttributeValues = device.AttributeValues.ToList()
            };
            return View(model);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var device = db.Devices.Find(id);
            if (device == null) return HttpNotFound();
            db.Devices.Remove(device);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                TempData["AddedProductName"] = device.DeviceName;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddToWishlist(int id)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            if (device != null)
            {
                _wishlistService.AddToWishlist(id, device);
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
            var cartItems = _cartService.GetCartItems();
            var wlItems = _wishlistService.GetWishlistItems();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            
            return View(new DetailsView { CartItems = cartItems, WishItems = wlItems, Device = device});
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
                        DeviceId = ci.DeviceId,
                        Quantity = ci.Quantity,
                        TotalAmount = ci.Device.Price
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
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
