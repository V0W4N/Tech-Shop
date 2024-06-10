using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.Models
{
    public class DeviceCategoryAttributeValue
    {
        [Key]
        public int ValueId { get; set; }
     
        [Required]
        [StringLength(100)]
        public string Value { get; set; }
        [ForeignKey("Device")] 
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }

        [ForeignKey("Attribute")]
        public int AttributeId { get; set; }
        public virtual DeviceCategoryAttribute Attribute { get; set; }
    }
}