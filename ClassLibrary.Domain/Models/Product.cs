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
    }
}
