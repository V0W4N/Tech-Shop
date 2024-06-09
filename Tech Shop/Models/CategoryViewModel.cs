using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Tech_Shop.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<DeviceCategoryAttribute> Attributes { get; set; }
    }
}