using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class ProductWithQuantity
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public ProductWithQuantity(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        
        public override bool Equals(object obj)
        {
            return obj is ProductWithQuantity productWithQuantity &&
                   Quantity == productWithQuantity.Quantity &&
                   Product.Equals(productWithQuantity.Product);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Product, Quantity);
        }
        
    }
}
