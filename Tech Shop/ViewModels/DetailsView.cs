using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.ViewModels
{

    public class DetailsView
    {
        public Device Device { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public IEnumerable<WishlistItem> WishItems { get; set; }
    }
}