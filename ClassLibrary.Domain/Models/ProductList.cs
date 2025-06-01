using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class ProductList
    {
        public int ShoppingListId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        public ProductList(int shoppingListId, int productId, int quantity)
        {
            ShoppingListId = shoppingListId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
