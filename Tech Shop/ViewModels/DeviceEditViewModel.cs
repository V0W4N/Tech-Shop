using Tech_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using Tech_Shop.Models;

namespace Tech_Shop.ViewModels
{
    public class DeviceEditViewModel
    {
        public DeviceEditViewModel()
        {
            // Set default values for non-object parameters
            DeviceId = 0;
            DeviceName = "";
            Manufacturer = "";
            Description = "";
            Price = 0.0m;
            CategoryId = 0;
            CategoryName = "";
            DeviceImage = "";
            Device = null;
            selectListItem = null;
            Attributes = new List<AttributeViewModel>();
        }

        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string DeviceImage { get; set; }
        public Device Device { get; set; }
        public SelectList selectListItem { get; set; }
        public List<AttributeViewModel> Attributes { get; set; }
    }
}
