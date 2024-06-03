using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.DBModel.Seed
{
    public class DeviceContext : DbContext
    {
        public DeviceContext() : base("name = Tech_Shop")
        {

        }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
    }
}