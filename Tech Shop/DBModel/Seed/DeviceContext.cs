using Online_Tech_Shop.Models;
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
        public virtual DbSet<DeviceCategory> DeviceCategories { get; set; }
        public virtual DbSet<DeviceCategoryAttribute> DeviceCategoryAttributes { get; set; }
        public virtual DbSet<DeviceCategoryAttributeValue> DeviceCategoryAttributeValues { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .HasRequired(d => d.Category)
                .WithMany(dc => dc.Devices)
                .HasForeignKey(d => d.CategoryId)
                .WillCascadeOnDelete(false); // Отключение каскадного удаления

            base.OnModelCreating(modelBuilder);
        }

    }
}