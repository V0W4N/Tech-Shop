using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.Services
{
    public class WishlistService
    {
        private readonly HttpContextBase _context;
        private bool _reqestedFromAccount = false;
        private ApplicationDbContext db = new ApplicationDbContext();

        public WishlistService(HttpContextBase context = null)
        {
            // If HttpContextBase is not provided, use the current HttpContext
            _context = context ?? new HttpContextWrapper(HttpContext.Current);
            List<WishlistItem> wList = _context.Session["Wishlist"] as List<WishlistItem>;
            bool isAuth = _context.User.Identity.IsAuthenticated;
            if ((wList == null || !wList.Any())
                || (isAuth != _reqestedFromAccount && isAuth))
            {
                if (isAuth)
                {
                    string userId = _context.User.Identity.GetUserId();
                    var data = db.WishlistData.SingleOrDefault(cd => cd.UserId == userId);
                    if (data != null) UnpackWishlistData(data.WishlistDataString);
                    else SetWishlistCookie();
                }
                else
                {
                    var cookie = _context.Request.Cookies["WishlistCookie"];
                    if (cookie != null)
                    {
                        UnpackWishlistData(cookie.Value);
                    }
                }
            }
        }

        private List<WishlistItem> Wishlist
        {
            get
            {
                var wList = _context.Session["Wishlist"] as List<WishlistItem> ?? new List<WishlistItem>();
                _context.Session["Wishlist"] = wList;
                return wList;
            }
            set { }
        }

        public IEnumerable<WishlistItem> GetWishlistItems()
        {
            return Wishlist;
        }

        public void AddToWishlist(int productId, Product product)
        {
            var WishlistItem = Wishlist.SingleOrDefault(c => c.ProductId == productId);
            if (WishlistItem == null)
            {
                Wishlist.Add(new WishlistItem { ProductId = productId, Product = product });
            }
            SetWishlistCookie();
        }
        public void RemoveFromWishlist(int productId)
        {
            var wishlistItem = Wishlist.SingleOrDefault(c => c.ProductId == productId);
            if (wishlistItem != null)
            {
               Wishlist.Remove(wishlistItem);
            }
            SetWishlistCookie();
        }

        public string CompactWishlistData()
        {
            return string.Join("|", Wishlist.Select(item => $"{item.ProductId}"));
        }

        public void UnpackWishlistData(string cartData)
        {
            Wishlist.Clear();
            var res = cartData.Split('=').Last();
            var items = res.Split('|');
            foreach (var item in items)
            {
                if (int.TryParse(item, out int productId))
                {
                    var product = db.Products.Find(productId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {productId} not found");
                    }

                    Wishlist.Add(new WishlistItem { ProductId = productId, Product = product });

                }
            }
            _context.Session["Wishlist"] = Wishlist;
        }
        public void EmptyWishlist()
        {
            Wishlist.Clear();
            SetWishlistCookie();
        }
        private void SetWishlistCookie()
        {
            string cookieData = CompactWishlistData();
            var cookie = new HttpCookie("WishlistCookie")
            {
                ["Products"] = cookieData,
                Expires = DateTime.MaxValue
            };
            _context.Response.Cookies.Add(cookie);

            if (_context.User.Identity.IsAuthenticated)
            {
                string userId = _context.User.Identity.GetUserId();

                var currWlData = db.WishlistData.SingleOrDefault(cd => cd.UserId == userId);

                if (currWlData != null)
                {
                    // User already has an entry, update it
                    currWlData.WishlistDataString = cookieData;
                    db.Entry(currWlData).State = EntityState.Modified;
                }
                else
                {
                    // User does not have an entry, add a new entry
                    db.WishlistData.Add(new WishlistData { UserId = userId, WishlistDataString = cookieData });
                }
                db.SaveChanges();
            }
        }
    }
}