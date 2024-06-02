using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Interfaces;
using Tech_Shop.Mocks;

namespace Tech_Shop.Models
{
    public class DeviceDbInitializer : DropCreateDatabaseAlways<DeviceContext>
    {
        private readonly IDeviceCategory _categoryDevice = new MockCategory();
        protected override void Seed(DeviceContext db)
        {
            db.Devices.Add(new Device
            {
                Name = "Acer",
                shortDescription = "Viser",
                description = "Ne lagaet v tetris",
                Price = 15000,
           //     category = _categoryDevice.AllCategories.First()
            });
            db.Devices.Add(new Device
            {
                Name = "BAss Fans",
                shortDescription = "Krutit",
                description = "Delaet vjuh",
                Price = 3000,
           //     category = _categoryDevice.AllCategories.Last()
            });
          

            base.Seed(db);
        }
    }
}