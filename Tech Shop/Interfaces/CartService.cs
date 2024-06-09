using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tech_Shop.DBModel.Seed;
using Tech_Shop.Models;

namespace Tech_Shop.Services
{
    public class CartService
    {
        private readonly HttpContextBase _context;
        private bool _reqestedFromAccount = false;
        private ApplicationDbContext _db = new ApplicationDbContext();
        private DeviceContext db = new DeviceContext();


        public CartService(HttpContextBase context = null)
        {
            // If HttpContextBase is not provided, use the current HttpContext
            _context = context ?? new HttpContextWrapper(HttpContext.Current);
            List<CartItem> cart = _context.Session["Cart"] as List<CartItem>;
            bool isAuth = _context.User.Identity.IsAuthenticated;
            if ((cart == null || !cart.Any()) 
                || (isAuth != _reqestedFromAccount && isAuth))
            {
                if (isAuth)
                {
                    string userId = _context.User.Identity.GetUserId();
                    var data = _db.CartData.SingleOrDefault(cd => cd.UserId == userId);
                    if (data != null) UnpackCartData(data.CartDataString);
                    else SetCartCookie();
                    }
                else {
                    UnpackCartData(_context.Request.Cookies["CartCookie"].Value);
                }
            } 
        }

        private List<CartItem> Cart
        {
            get
            {
                var cart = _context.Session["Cart"] as List<CartItem> ?? new List<CartItem>();
                _context.Session["Cart"] = cart;
                return cart;
            }
            set { }
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            return Cart;
        }

        public void AddToCart(int deviceId, Device device)
        {
            var cartItem = Cart.SingleOrDefault(c => c.DeviceId == deviceId);
            if (cartItem == null)
            {
                Cart.Add(new CartItem { DeviceId = deviceId, Quantity = 1 , Device = device});
            }
            else
            {
                cartItem.Quantity++;
            }
            SetCartCookie();
        }
        public void UpdateCart(int deviceId, int quantity)
        {
            var cartItem = Cart.SingleOrDefault(c => c.DeviceId == deviceId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }
            SetCartCookie();
        }
        public void RemoveFromCart(int deviceId)
        {
            var cartItem = Cart.SingleOrDefault(c => c.DeviceId == deviceId);
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                   cartItem.Quantity--;
                }
                else
                {
                    Cart.Remove(cartItem);
                }
            }
            SetCartCookie();
        }

        public void DeleteFromCart(int deviceId)
        {
            var cartItem = Cart.SingleOrDefault(c => c.DeviceId == deviceId);
            if (cartItem != null)
            {
               Cart.Remove(cartItem);
            }
            SetCartCookie();
        }
        public string CompactCartData()
        {
            return string.Join("|", Cart.Select(item => $"{item.DeviceId}-{item.Quantity}"));
        }

        public void UnpackCartData(string cartData)
        {
            Cart.Clear();
            var res = cartData.Split('=').Last();
            var items = res.Split('|');
            foreach (var item in items)
            {
                var parts = item.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int deviceId) && int.TryParse(parts[1], out int quantity))
                {
                    var device = db.Devices.Find(deviceId);
                    if (device == null)
                    {
                        continue;
                    }

                    Cart.Add(new CartItem { DeviceId = deviceId, Quantity = quantity, Device = device });

                }
            }
            _context.Session["Cart"] = Cart;
        }
        public void EmptyCart()
        {
            Cart.Clear();
            SetCartCookie();
        }
        private void SetCartCookie()
        {
            string cookieData = CompactCartData();
            var cookie = new HttpCookie("CartCookie")
            {
                ["Devices"] = cookieData,
                Expires = DateTime.MaxValue
            };
            _context.Response.Cookies.Add(cookie);

            if (_context.User.Identity.IsAuthenticated)
            {
                string userId = _context.User.Identity.GetUserId();

                var currCartData = _db.CartData.SingleOrDefault(cd => cd.UserId == userId);

                if (currCartData != null)
                {
                    // User already has an entry, update it
                    currCartData.CartDataString = cookieData;
                    _db.Entry(currCartData).State = EntityState.Modified;
                }
                else
                {
                    // User does not have an entry, add a new entry
                    _db.CartData.Add(new CartData { UserId = userId, CartDataString = cookieData });
                }
                _db.SaveChanges();
            }
        }
    }
}