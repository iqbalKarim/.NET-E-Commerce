using System;

namespace API.Entities
{
    public class Product
    {
        public Product(int id, string ProductName, string Category, double Price){
            this.Id = id;
            this.ProductName = ProductName;
            this.Category = Category;
            this.Price = Price;
        }
        public Product(int id, string ProductName, string Category){
            this.Id = id;
            this.ProductName = ProductName;
            this.Category = Category;
            this.Price = 0.00;
        }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        // public string Description { get; set; }
    }
}