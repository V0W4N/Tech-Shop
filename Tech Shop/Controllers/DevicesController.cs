using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Mvc;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Interfaces;
using Tech_Shop.Mocks;
using Tech_Shop.Models;

namespace Tech_Shop.Controllers
{

    public class DevicesController : Controller
    {

        DeviceContext db = new DeviceContext();
        public DevicesController() { }
        //public ViewResult List()
        //{
        //    IEnumerable<Device> devices = db.Devices;
        //    ViewBag.Devices = devices;
        //    return View();
        //}
        
        [HttpGet]
        public ActionResult Buy(int id)
        {
            if (id == 2)
            {
              return  Redirect("/Home/About");
            }
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
        public ActionResult List(string name)
        {
            IQueryable<Device> devices = db.Devices;
          
            if (!String.IsNullOrEmpty(name) && !name.Equals("Все"))
            {
                devices = devices.Where(p => p.Name == name);
            }
            List<string> devicesName = db.Devices.Select(p=> p.Name).ToList();
            devicesName.Insert(0, "Все");
            DeviceListViewModel dlvm = new DeviceListViewModel
            {
                Devices = devices.ToList(),
                Names = new SelectList(devicesName, "Name")
            };
            return View(dlvm);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}