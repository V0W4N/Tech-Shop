using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Interfaces;
using Tech_Shop.Mocks;
using Tech_Shop.Models;
using Tech_Shop.ViewModels;

namespace Tech_Shop.Controllers
{

    public class DevicesController : Controller
    {

        DeviceContext db = new DeviceContext();
        public DevicesController() { }
        public ViewResult List()
        {
            IEnumerable<Device> devices = db.Devices;
            ViewBag.Devices = devices;
            return View();
        }
        
        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.DeviceId = id;
            return View();
        }
        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            db.Purchases.Add(purchase);
            db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
        }
    }
}