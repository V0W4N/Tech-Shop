using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    
        public class AttributeViewModel
        {
            public int AttributeId { get; set; }
            public string AttributeName { get; set; }
            public DeviceCategoryAttributeValue AttributeValue { get; set; }
        }
}