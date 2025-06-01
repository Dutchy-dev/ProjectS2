using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;
/*
namespace ClassLibrary.Application.Services
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
        
        public List<(Product product, int quantity)> GetShoppingListDetails(int shoppingListId)
        {
            return _productListRepository.GetProductsByShoppingListId(shoppingListId);
        }

        public List<(ShoppingList shoppingList, List<(Product product, int quantity)> products)> GetShoppingListsWithProductsByUser(int userId)
        {
            var ShoppingLists = _shoppingListRepository.GetShoppingListsByUserId(userId);

            var result = new List<(ShoppingList, List<(Product, int)>)>();

            foreach (var list in ShoppingLists)
            {
                var productsWithQuantities = _productListRepository.GetProductsByShoppingListId(list.Id);
                result.Add((list, productsWithQuantities));
            }

            return result;
        }

        public void CreateShoppingList(string theme, int userId)
        {
            var list = new ShoppingList
            {
                Theme = theme,
                UserId = userId
            };
            _shoppingListRepository.Add(list);
        }

        public void DeleteList(int shoppingListId)
        {
            _shoppingListRepository.DeleteShoppingList(shoppingListId);
        }

    }
}
*/