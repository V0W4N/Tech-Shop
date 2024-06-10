using Tech_Shop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class DeviceCategory
    {
        
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<DeviceCategoryAttribute> CategoryAttributes { get; set; } = new List<DeviceCategoryAttribute>();   
    }
}