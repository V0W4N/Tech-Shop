using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class WishlistItem
    {
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public WishlistItem() { }
    }
    public class WishlistData
    {
        [Key]
        public string UserId { get; set; }
        public string WishlistDataString { get; set; }
    }
}