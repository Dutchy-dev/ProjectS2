using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;

namespace ClassLibrary.Domain.Services
{
    public class ShoppingListService
    {
        private readonly IShoppingListRepo _shoppingListRepository;
        private readonly IProductListRepo _productListRepository;


        public ShoppingListService(IShoppingListRepo shoppingListRepository, IProductListRepo productListRepository)
        {
            _shoppingListRepository = shoppingListRepository;
            _productListRepository = productListRepository;
        }
        
        public List<ProductWithQuantity> GetShoppingListDetails(int shoppingListId)
        {
            return _productListRepository.GetProductsByShoppingListId(shoppingListId);
        }

        public List<(ShoppingList shoppingList, List<ProductWithQuantity> products)> GetShoppingListsWithProductsByUser(int userId)
        {
            var ShoppingLists = _shoppingListRepository.GetShoppingListsByUserId(userId);

            var result = new List<(ShoppingList, List<ProductWithQuantity>)>();

            foreach (var list in ShoppingLists)
            {
                var productsWithQuantities = _productListRepository.GetProductsByShoppingListId(list.Id);
                result.Add((list, productsWithQuantities));
            }

            return result;
        }

        public void CreateShoppingList(string theme, int userId)
        {
            var list = new ShoppingList(theme, userId);
            _shoppingListRepository.Add(list);
        }

        public void DeleteList(int shoppingListId)
        {
            _shoppingListRepository.DeleteShoppingList(shoppingListId);
        }

    }
}
