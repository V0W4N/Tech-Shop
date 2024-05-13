using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int ProductId { get; set; }
        public Product() { }
    }
    public class ProductList
    {
        public IEnumerable<Product> Products { get; set; }
        public Boolean IsAdmin { get; set; }
    }
    public class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}