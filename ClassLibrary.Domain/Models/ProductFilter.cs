using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class ProductFilter
    {
        public string? Name { get; private set; }
        public string? Store { get; private set; }
        public string? Category { get; private set; }

        public ProductFilter (string? name, string? store, string? category) // Constructor voor de ViewModel
        {
            Name = name;
            Store = store;
            Category = category;
        }
    }
}
