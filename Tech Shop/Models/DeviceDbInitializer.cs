using Online_Tech_Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using Tech_Shop.DBModel.Seed;

namespace Tech_Shop.Models
{
    public class DeviceDbInitializer : CreateDatabaseIfNotExists<DeviceContext>
    {
        protected override void Seed(DeviceContext db)
        {
            // Создание категорий
            var category1 = new DeviceCategory { CategoryName = "Ноутбук" };
            var category2 = new DeviceCategory { CategoryName = "Камера" };
            var category3 = new DeviceCategory { CategoryName = "Вентилятор" };

            // Добавление категорий
            db.DeviceCategories.Add(category1);
            db.DeviceCategories.Add(category2);
            db.DeviceCategories.Add(category3);

            db.SaveChanges();

            // Создание атрибутов
            var attribute1 = new DeviceCategoryAttribute { AttributeName = "Мощность" };
            var attribute2 = new DeviceCategoryAttribute { AttributeName = "Скорость" };
            var attribute3 = new DeviceCategoryAttribute { AttributeName = "Размер" };

            // Добавление атрибутов
            db.DeviceCategoryAttributes.Add(attribute1);
            db.DeviceCategoryAttributes.Add(attribute2);
            db.DeviceCategoryAttributes.Add(attribute3);

            db.SaveChanges();

            // Привязка атрибутов к категориям
            category1.CategoryAttributes.Add(attribute1);
            category1.CategoryAttributes.Add(attribute2);
            category2.CategoryAttributes.Add(attribute2);
            category3.CategoryAttributes.Add(attribute3);

            db.SaveChanges();

            // Создание устройств
            var device1 = new Device
            {
                DeviceName = "Extensa",
                Manufacturer = "Viser",
                Description = "Не лагает в тетрис",
                Price = 15000,
                CategoryId = category1.CategoryId
            };
            var device2 = new Device
            {
                Manufacturer = "BAss Fans",
                DeviceName = "Krutit",
                Description = "Делает вжух",
                Price = 3000,
                CategoryId = category3.CategoryId
            };
            var device3 = new Device
            {
                Manufacturer = "Sony",
                DeviceName = "Visit",
                Description = "Долго висит",
                Price = 2700,
                CategoryId = category2.CategoryId
            };

            // Добавление устройств
            db.Devices.Add(device1);
            db.Devices.Add(device2);
            db.Devices.Add(device3);

            db.SaveChanges();

            // Создание значений атрибутов для устройств
            var attributeValue1 = new DeviceCategoryAttributeValue
            {
                DeviceId = device1.DeviceId,
                AttributeId = attribute1.AttributeId,
                Value = "1000 Вт"
            };
            var attributeValue2 = new DeviceCategoryAttributeValue
            {
                DeviceId = device1.DeviceId,
                AttributeId = attribute2.AttributeId,
                Value = "5000 об/мин"
            };
            var attributeValue3 = new DeviceCategoryAttributeValue
            {
                DeviceId = device2.DeviceId,
                AttributeId = attribute2.AttributeId,
                Value = "2000 об/мин"
            };
            var attributeValue4 = new DeviceCategoryAttributeValue
            {
                DeviceId = device3.DeviceId,
                AttributeId = attribute3.AttributeId,
                Value = "40x40x20 мм"
            };

            // Добавление значений атрибутов
            db.DeviceCategoryAttributeValues.Add(attributeValue1);
            db.DeviceCategoryAttributeValues.Add(attributeValue2);
            db.DeviceCategoryAttributeValues.Add(attributeValue3);
            db.DeviceCategoryAttributeValues.Add(attributeValue4);

            db.SaveChanges();

            base.Seed(db);
        }
    }
}
