using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tech_Shop.Interfaces;
using Tech_Shop.Models;

namespace Tech_Shop.Mocks
{
    public class MockCategory : IDeviceCategory
    {
        public IEnumerable<DeviceCategory> AllCategories { 
            get {
                return new List<DeviceCategory>()
                {
                    new DeviceCategory { Name = "Ноутбуки", Description = "Комп с собой" },
                    new DeviceCategory { Name =  "Вентиляторы", Description = "Вжжжжжжж"}
                };
            }
        }
    }
}