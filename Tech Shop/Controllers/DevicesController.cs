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
       
        //IAllDevices _allDevices = new MockDevices();
        //IDeviceCategory _allCategories = new MockCategory();
        //public DevicesController(MockDevices iAllDevices, MockCategory iDeviceCat)
        //{
        //    _allDevices = iAllDevices;
        //    _allCategories = iDeviceCat;
        //}
        public DevicesController() { }
        public ViewResult List()
        {
            //DeviceListViewModel obj = new DeviceListViewModel();
            //obj.allDevices = db.Devices;
            //obj.currCategory = "Устройства";
            IEnumerable<Device> devices = db.Devices;
            ViewBag.Devices = devices;
            return View();
        }
    }
}