using Tech_Shop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace Tech_Shop.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }
        [Required]
        [StringLength(100)] 
        public string DeviceName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [StringLength(100)] 
        public string Manufacturer { get; set; }
        [Required] public string Description { get; set; }
        public string DeviceImage { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual DeviceCategory Category { get; set; }
        public virtual ICollection<DeviceCategoryAttributeValue> AttributeValues{ get; set ;}
    }

}