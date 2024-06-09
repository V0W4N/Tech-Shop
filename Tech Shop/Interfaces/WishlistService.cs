using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Models;

namespace Tech_Shop.Services
{
    public class WishlistService
    {
        private readonly HttpContextBase _context;
        private bool _reqestedFromAccount = false;
        private ApplicationDbContext _db = new ApplicationDbContext();
        private DeviceContext db = new DeviceContext();

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
                    var data = _db.WishlistData.SingleOrDefault(cd => cd.UserId == userId);
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

        public void AddToWishlist(int deviceId, Device device)
        {
            var WishlistItem = Wishlist.SingleOrDefault(c => c.DeviceId == deviceId);
            if (WishlistItem == null)
            {
                Wishlist.Add(new WishlistItem {DeviceId = deviceId, Device = device });
            }
            SetWishlistCookie();
        }
        public void RemoveFromWishlist(int deviceId)
        {
            var wishlistItem = Wishlist.SingleOrDefault(c => c.DeviceId == deviceId);
            if (wishlistItem != null)
            {
               Wishlist.Remove(wishlistItem);
            }
            SetWishlistCookie();
        }

        public string CompactWishlistData()
        {
            return string.Join("|", Wishlist.Select(item => $"{item.DeviceId}"));
        }

        public void UnpackWishlistData(string cartData)
        {
            Wishlist.Clear();
            var res = cartData.Split('=').Last();
            var items = res.Split('|');
            foreach (var item in items)
            {
                if (int.TryParse(item, out int deviceId))
                {
                    var device = db.Devices.Find(deviceId);
                    if (device == null)
                    {
                        continue;
                    }

                    Wishlist.Add(new WishlistItem { DeviceId = deviceId, Device = device });

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
                ["Wishlist"] = cookieData,
                Expires = DateTime.MaxValue
            };
            _context.Response.Cookies.Add(cookie);

            if (_context.User.Identity.IsAuthenticated)
            {
                string userId = _context.User.Identity.GetUserId();

                var currWlData = _db.WishlistData.SingleOrDefault(cd => cd.UserId == userId);

                if (currWlData != null)
                {
                    // User already has an entry, update it
                    currWlData.WishlistDataString = cookieData;
                    _db.Entry(currWlData).State = EntityState.Modified;
                }
                else
                {
                    // User does not have an entry, add a new entry
                    _db.WishlistData.Add(new WishlistData { UserId = userId, WishlistDataString = cookieData });
                }
                _db.SaveChanges();
            }
        }
    }
}