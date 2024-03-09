using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tech_Shop.Interfaces;
using Tech_Shop.Models;

namespace Tech_Shop.Mocks
{
    public class MockDevices : IAllDevices {
        private readonly IDeviceCategory _categoryDevice = new MockCategory();
        public IEnumerable<Device> devices {
            get {
                return new List<Device> {
                    new Device
                    {
                        Name = "Acer",
                        shortDescription = "Viser",
                        description = "Ne lagaet v tetris",
                        Price = 15000,
                        category = _categoryDevice.AllCategories.First()
                    },
                    new Device
                    {
                        Name = "BAss Fans",
                        shortDescription = "Krutit",
                        description = "Delaet vjuh",
                        Price = 3000,
                        category = _categoryDevice.AllCategories.Last()
                    }
                };
            }
          
        }

        public Device getDeviceByID(int deviceId)
        {
            throw new NotImplementedException();
        }
    }
}