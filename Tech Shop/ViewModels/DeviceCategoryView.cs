using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.ViewModels
{
    public class DeviceCategoryView
    {
        public List<DeviceCategory> DeviceCategories { get; set; }
        public string NewCategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}