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

        List<ShoppingList> GetShoppingListsByUserId(int userId);

        void DeleteShoppingList(int shoppingListId);

    }
}
