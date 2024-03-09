using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ushort Price { get; set; }
        public string shortDescription { get; set; }
        public string description { get; set; }
        public DeviceCategory category { get; set; }
    }
}