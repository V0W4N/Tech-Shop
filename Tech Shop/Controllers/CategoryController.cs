using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = db.DeviceCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            CategoryViewModel model = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Attributes = category.CategoryAttributes.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
                var category = db.DeviceCategories.Find(model.CategoryId);
                if (category != null)
                {
                    category.CategoryName = model.CategoryName;

                    // Update existing attributes
                    foreach (var attributeModel in model.Attributes)
                    {
                        var attribute = db.DeviceCategoryAttributes.Find(attributeModel.AttributeId);
                        if (attribute != null)
                        {
                            attribute.AttributeName = attributeModel.AttributeName;
                        }
                    }

                    db.SaveChanges();
                }
            return View(model);
        }


        [HttpPost]
        public ActionResult AddAttribute(CategoryViewModel model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.NewAttributeName))
            {
                var category = db.DeviceCategories.Find(model.CategoryId);
                if (category != null)
                {
                    var newAttribute = new DeviceCategoryAttribute
                    {
                        AttributeName = model.NewAttributeName,
                        CategoryId = model.CategoryId
                    };
                    db.DeviceCategoryAttributes.Add(newAttribute);
                    db.SaveChanges(); // Save the new attribute to get its ID

                    category.CategoryAttributes.Add(newAttribute); // Tie the attribute to the category
                    db.SaveChanges(); // Save changes again to ensure relationship is saved
                }
                return RedirectToAction("Edit", new { id = model.CategoryId });
            }
            // Log or inspect ModelState to see validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            }
            return RedirectToAction("Edit", new { id = model.CategoryId });
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
