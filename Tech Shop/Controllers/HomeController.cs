using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech_Shop.Models;

namespace Tech_Shop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cookie = Request.Cookies["CustomCookie"];
            string cookieValue = cookie != null ? cookie.Value : "Cookie not found";
            ViewBag.CookieValue = cookieValue;
            return View();
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
    }
}