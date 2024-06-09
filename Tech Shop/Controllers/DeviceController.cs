using Online_Tech_Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Models;
using Tech_Shop.ViewModels;

namespace Tech_Shop.Controllers
{

    public class DeviceController : Controller
    {

        DeviceContext db = new DeviceContext();
        //public DeviceController() { }

        //[HttpGet]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)             
        //        return HttpNotFound();
        //    Device device = db.Devices.Find(id);
        //    if (device != null) 
        //        return View(device);
        //    return HttpNotFound();
        //}
        //[HttpPost]
        //public ActionResult Edit(Device device)
        //{
        //    db.Entry(device).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("List");
        //}
        //[HttpGet]
        //public ActionResult Create() {
        //    var devices = db.Devices.Include(d => d.Category)
        //                    .Include(d => d.AtributeValues.Select(av => av.Attribute))
        //                    .ToList();
        //    return View(devices);
        //}

        //[HttpPost]
        //public ActionResult Create(Device device)
        //{
        //    db.Devices.Add(device);
        //    db.SaveChanges();
        //    return RedirectToAction("List");
        //}
        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    Device device = db.Devices.Find(id);    
        //    if(id == null) return HttpNotFound();
        //    return View(device);
        //}
        //[HttpPost , ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int? id)
        //{
        //    Device device = db.Devices.Find(id);
        //    if (id == null) return HttpNotFound();
        //    db.Devices.Remove(device);
        //    db.SaveChanges();
        //    return RedirectToAction("List");
        //}


        //public ActionResult List(string name)
        //{
        //    IQueryable<Device> devices = db.Devices;

        //    if (!String.IsNullOrEmpty(name) && !name.Equals("Все"))
        //    {
        //        devices = devices.Where(p => p.DeviceName == name);
        //    }
        //    List<string> devicesName = db.Devices.Select(p=> p.DeviceName).ToList();
        //    devicesName.Insert(0, "Все");
        //    DeviceListViewModel dlvm = new DeviceListViewModel
        //    {
        //        Devices = devices.ToList(),
        //        Names = new SelectList(devicesName, "Name")
        //    };
        //    return View(dlvm);
        //}
        public ActionResult Index()
        {
            var devices = db.Devices.Include(d => d.Category).Include(d => d.AttributeValues).ToList();
            var model = devices.Select(d => new DeviceViewModel
            {
                DeviceId = d.DeviceId,
                DeviceName = d.DeviceName,
                Manufacturer = d.Manufacturer,
                Description = d.Description,
                Price = d.Price,
                CategoryId = d.CategoryId,
                CategoryName = d.Category.CategoryName,
                AttributeValues = d.AttributeValues.ToList()
            }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.DeviceCategories, "CategoryId", "CategoryName");
            ViewBag.Attributes = db.DeviceCategoryAttributes.Select(a => new AttributeViewModel
            {
                AttributeId = a.AttributeId,
                AttributeName = a.AttributeName
            }).ToList();
            return View(new DeviceViewModel { AttributeValues = new List<DeviceCategoryAttributeValue>() });
        }

        // POST: Device/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeviceViewModel deviceViewModel)
        {
            if (ModelState.IsValid)
            {
                var device = new Device
                {
                    DeviceName = deviceViewModel.DeviceName,
                    Manufacturer = deviceViewModel.Manufacturer,
                    Description = deviceViewModel.Description,
                    Price = deviceViewModel.Price,
                    CategoryId = deviceViewModel.CategoryId,
                    AttributeValues = deviceViewModel.AttributeValues.Select(a => new DeviceCategoryAttributeValue
                    {
                        AttributeId = a.AttributeId,
                        Value = a.Value
                    }).ToList()
                };

                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.DeviceCategories, "CategoryId", "CategoryName", deviceViewModel.CategoryId);
            ViewBag.Attributes = db.DeviceCategoryAttributes.Select(a => new AttributeViewModel
            {
                AttributeId = a.AttributeId,
                AttributeName = a.AttributeName
            }).ToList();
            return View(deviceViewModel);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            var device = db.Devices.Find(id);
            if (device == null) return HttpNotFound();
            List<AttributeViewModel> attributes = new List<AttributeViewModel>();
            foreach (var attribute in device.AttributeValues)
            {
                attributes.Add(new AttributeViewModel() 
                {
                    AttributeId = attribute.AttributeId, 
                    AttributeName = attribute.Attribute.AttributeName,
                    AttributeValue = attribute.Value
                });

                
            }
            var model = new DeviceEditViewModel
            {
                DeviceId = device.DeviceId,
                DeviceName = device.DeviceName,
                Manufacturer = device.Manufacturer,
                Description = device.Description,
                Price = device.Price,
                CategoryId = device.CategoryId,
                CategoryName = device.Category.CategoryName,
                Attributes = attributes,
                AttributeValues = device.AttributeValues.ToList()
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
        public ActionResult Edit(DeviceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var device = db.Devices.Find(model.DeviceId);
                if (device == null) return HttpNotFound();
                device.DeviceName = model.DeviceName;
                device.Manufacturer = model.Manufacturer;
                device.Description = model.Description;
                device.Price = model.Price;
                device.CategoryId = model.CategoryId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.DeviceCategories, "CategoryId", "CategoryName", model.CategoryId);
            return View(model);
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
        protected override void Dispose(bool disposing)
        {
            {
                db.Dispose();
                base.Dispose(disposing);
            }
        }
    }

}
