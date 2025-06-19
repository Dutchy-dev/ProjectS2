using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Store { get; private set; }
        public decimal Price { get; private set; }
        public string Category { get; private set; }

        public Product(int id, string name, string store, decimal price, string category)
        {
            Id = id;
            Name = name;
            Store = store;
            Price = price;
            Category = category;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Id == product.Id &&
                   Name == product.Name &&
                   Store == product.Store &&
                   Price == product.Price &&
                   Category == product.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Store, Price, Category);
        }
        
    }
}
