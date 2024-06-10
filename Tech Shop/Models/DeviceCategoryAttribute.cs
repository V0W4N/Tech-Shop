using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.Models
{
    public class DeviceCategoryAttribute
    {
        [Key]
        public int AttributeId {  get; set; }
        [Required]
        [StringLength(100)]
        public string AttributeName { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual ICollection<DeviceCategory> Category { get; set; } = new List<DeviceCategory>();
        public virtual ICollection<DeviceCategoryAttributeValue> AttributeValues { get; set; } = new List<DeviceCategoryAttributeValue>();



    }
}