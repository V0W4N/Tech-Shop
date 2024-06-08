using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tech_Shop.Models;

namespace Tech_Shop.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public CartItem() { }
        public CartItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
    public class CartData
    {
        [Key]
        public string UserId { get; set; }
        public string CartDataString { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }

}