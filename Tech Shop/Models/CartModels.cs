using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class CartItem
    {
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int Quantity { get; set; }
        public CartItem() { }
    }
    public class CartData
    {
        [Key]
        public string UserId { get; set; }
        public string CartDataString { get; set; }
    }
}