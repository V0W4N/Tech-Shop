using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class DeviceViewModel
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<DeviceCategoryAttributeValue> AttributeValues { get; set; }
    }
}