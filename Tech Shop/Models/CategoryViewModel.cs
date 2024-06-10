using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string NewCategoryName { get; set; }
        public string NewAttributeName { get; set; }
        public List<DeviceCategoryAttribute> Attributes { get; set; }
    }
}