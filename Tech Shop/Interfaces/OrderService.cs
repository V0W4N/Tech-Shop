using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.Services
{
    public class OrderService
    {

        private readonly HttpContextBase _context;
        private bool _reqestedFromAccount = false;
        private readonly CartService _cartService = new CartService();
        private ApplicationDbContext _db = new ApplicationDbContext();

        public OrderService(HttpContextBase context = null)
        {
            // If HttpContextBase is not provided, use the current HttpContext
            _context = context ?? new HttpContextWrapper(HttpContext.Current);
        }
            public IEnumerable<Order> GetOrdersByUser(string userId)
        {
            return _db.Orders.Where(o => o.UserId == userId).Include(o => o.OrderItems).ToList();
        }

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public void AddOrderItem(int orderId, OrderItem orderItem)
        {
            var order = _db.Orders.Find(orderId);
            if (order == null)
                throw new InvalidOperationException("Order not found");

            order.OrderItems.Add(orderItem);
            _db.SaveChanges();
        }

        public bool ConfirmPurchase()
        {
            return _cartService.GetCartItems().Any();
        }
    }
}
