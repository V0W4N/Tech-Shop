using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class WishlistItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public WishlistItem() { }
        public WishlistItem(int productId)
        {
            ProductId = productId;
        }
    }
    public class WishlistData
    {
        [Key]
        public string UserId { get; set; }
        public string WishlistDataString { get; set; }
    }
}