using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.ViewModels
{
    public class DeviceListViewModel
    {
        public IEnumerable<Device> allDevices { get; set; }
        public string currCategory = "Устройства";
    }
}