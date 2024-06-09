using Online_Tech_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tech_Shop.ViewModels
{
    /*
     var device = new Device
                {
                    DeviceName = deviceViewModel.DeviceName,
                    Manufacturer = deviceViewModel.Manufacturer,
                    Description = deviceViewModel.Description,
                    Price = deviceViewModel.Price,
                    CategoryId = deviceViewModel.CategoryId,
                    AttributeValues = deviceViewModel.AttributeValues.Select(a => new DeviceCategoryAttributeValue
                    {
                        AttributeId = a.AttributeId,
                        Value = a.Value
                    }).ToList()
                };
    */
    public class DeviceCreateViewModel
    {

            public int DeviceId { get; set; }
            public string DeviceName { get; set; }
            public string Manufacturer { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public SelectList selectListItem { get; set; }
            public List<AttributeViewModel> Attributes { get; set; }
    }
}