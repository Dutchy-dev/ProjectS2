using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Interfaces
{
    public interface IProductListRepo
    {
        void AddProductToList(ProductList item);

        List<ProductWithQuantity> GetProductsByShoppingListId(int shoppingListId);

        void RemoveProductFromList(int shoppingListId, int productId);

        void UpdateQuantity(int shoppingListId, int productId, int delta);

        List<Product> GetFilteredProducts(ProductFilter filter);
    }
}
