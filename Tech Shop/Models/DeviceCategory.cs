using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class DeviceCategory
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Device> devices { get; set; }

    }
}