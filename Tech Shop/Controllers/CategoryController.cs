using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Models;
using Tech_Shop.ViewModels;

namespace Tech_Shop.Controllers
{

    [AdminAuthorize(Roles = "Moderator,Admin,PowerUser")]
    public class CategoryController : Controller
    {
        DeviceContext db = new DeviceContext();

        [HttpGet]
        public ActionResult List()
        {
            var model = new DeviceCategoryView { DeviceCategories = db.DeviceCategories.ToList() };
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            var category = db.DeviceCategories.Find(id);
            if (category == null) return HttpNotFound();
            CategoryViewModel model = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Attributes = category.CategoryAttributes.ToList()
            };
            return View(model);
        }

        public ActionResult EditCategory(CategoryViewModel model)
        {
            var category = db.DeviceCategories.Find(model.CategoryId);
            if (category != null)
                {
                    category.CategoryName = model.CategoryName;
                    category.CategoryId = model.CategoryId;

                var existingAttributes = db.DeviceCategoryAttributes
                 .Where(av => av.CategoryId == model.CategoryId);

                foreach (var existingValue in existingAttributes)
                {
                    db.DeviceCategoryAttributes.Remove(existingValue);
                }
                List<DeviceCategoryAttribute> attributeValues = new List<DeviceCategoryAttribute>();
                if (model.Attributes != null)
                {
                    foreach (var item in model.Attributes)
                    {
                        attributeValues.Add(new DeviceCategoryAttribute
                        {
                            AttributeName = item.AttributeName,
                            AttributeId = item.AttributeId,
                            CategoryId = category.CategoryId
                        });
                    }
                }
                category.CategoryAttributes = attributeValues;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                }
            return View("Edit", model);
        }

        
        [HttpPost]
        public ActionResult AddAttribute(CategoryViewModel model)
        {
            var category = db.DeviceCategories.Find(model.CategoryId);
            if (category != null)
            {
                var newAttr = new DeviceCategoryAttribute
                {
                    AttributeName = model.NewAttributeName,
                    CategoryId = category.CategoryId
                };
                if (model.Attributes == null)
                {
                    model.Attributes = new List<DeviceCategoryAttribute>();
                }

                model.Attributes.Add(newAttr);
                category.CategoryAttributes.Add(newAttr);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View("Edit", model);
        }

        [HttpPost]
        public void RemoveAttribute(int AttributeId, int CategoryId)
        {
            var attribute = db.DeviceCategoryAttributes.Find(AttributeId);
            if (attribute != null)
            {
                var category = db.DeviceCategories.Find(CategoryId);
                if (category != null)
                {
                    category.CategoryAttributes.Remove(attribute);
                    db.DeviceCategoryAttributes.Remove(attribute);
                    db.SaveChanges();
                }
            }
        }


        [HttpPost]
        public ActionResult AddCategory(DeviceCategoryView model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.NewCategoryName))
            {
                var existingCategory = db.DeviceCategories.FirstOrDefault(c => c.CategoryName == model.NewCategoryName);

                if (existingCategory == null && model.NewCategoryName != null)
                {
                    var newCategory = new DeviceCategory
                    {
                        CategoryName = model.NewCategoryName
                    };
                    db.DeviceCategories.Add(newCategory);
                    db.SaveChanges(); // Save the new attribute to get its ID
                }
                return RedirectToAction("List");
            }
            // Log or inspect ModelState to see validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            }
            return RedirectToAction("List");
        }
    }
}
