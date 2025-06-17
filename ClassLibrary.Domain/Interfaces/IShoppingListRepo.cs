using ClassLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Interfaces
{
    public interface IShoppingListRepo
    {
        void Add(ShoppingList list);

        int GetProductQuantity(int shoppingListId, int productId);

        void SaveProductListItem(int shoppingListId, int productId, int quantity);

        List<ShoppingList> GetShoppingListsByUserId(int userId);

        void DeleteShoppingList(int shoppingListId);

        ShoppingList GetShoppingListById(int shoppingListId);
    }
}
