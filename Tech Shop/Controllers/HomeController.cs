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
using System.Xml.Linq;


namespace Tech_Shop.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private DeviceContext db = new DeviceContext();
        private CartService _cartService = new CartService();
        private OrderService _orderService = new OrderService();
        private WishlistService _wishlistService = new WishlistService();
        private List<string> roleList = new List<string> { "Moderator", "Admin", "PowerUser" };
        public ActionResult Index(string name)
        {
            var devices = db.Devices.Include(d => d.Category).Include(d => d.AttributeValues).ToList();
            var cartItems = _cartService.GetCartItems();
            var wlItems = _wishlistService.GetWishlistItems();


            IQueryable<Device> deviceQ = db.Devices;
            if (!String.IsNullOrEmpty(name) && !name.Equals("Все"))
            {
                deviceQ = deviceQ.Where(p => p.Category.CategoryName == name);
            }
            //List<DeviceCategory> devicesName = db.DeviceCategories.Where(p => p.CategoryName == name).ToList();

            List<string> deviceCategories = db.DeviceCategories.Select(p => p.CategoryName).ToList();
            deviceCategories.Insert(0, "Все");
            DeviceListViewModel dlvm = new DeviceListViewModel
            {
                Devices = deviceQ.ToList(),
                Categories = new SelectList(deviceCategories, "CategoryName")
            };


            var list = new ProductListWithQ
            {
                Devices = devices,
                CartItems = cartItems,
                WishItems = wlItems,
                DeviceListViewModel = dlvm,
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Support()
        {
            ViewBag.Message = "Create a support request";
            SupportFormModel model = new SupportFormModel();
            return View(model);
        }
        public ActionResult Products()
        {
            return View();
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