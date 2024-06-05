using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech_Shop.Models;

namespace Tech_Shop.Models
{
    public class DeviceListViewModel
    {
        public IEnumerable<Device> Devices { get; set; }
        public SelectList Names { get; set; }
    }
}