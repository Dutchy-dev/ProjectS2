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
        private readonly IShoppingListRepo _shoppingListRepo;
        private readonly IProductListRepo _productListRepo;


        public ShoppingListService(IShoppingListRepo shoppingListRepository, IProductListRepo productListRepository)
        {
            _shoppingListRepo = shoppingListRepository;
            _productListRepo = productListRepository;
        }
        
        public List<ProductWithQuantity> GetShoppingListDetails(int shoppingListId)
        {
            return _productListRepo.GetProductsByShoppingListId(shoppingListId);
        }

        public ShoppingList GetShoppingListById(int shoppingListId)
        {
            return _shoppingListRepo.GetShoppingListById(shoppingListId);
        }

        public List<(ShoppingList shoppingList, List<ProductWithQuantity> products)> GetShoppingListsWithProductsByUser(int userId)
        {
            var ShoppingLists = _shoppingListRepo.GetShoppingListsByUserId(userId);

            var result = new List<(ShoppingList, List<ProductWithQuantity>)>();

            foreach (var list in ShoppingLists)
            {
                var productsWithQuantities = _productListRepo.GetProductsByShoppingListId(list.Id);
                result.Add((list, productsWithQuantities));
            }

            return result;
        }

        public void CreateShoppingList(string theme, int userId)
        {
            var list = new ShoppingList(theme, userId);
            _shoppingListRepo.Add(list);
        }

        public void DeleteList(int shoppingListId)
        {
            _shoppingListRepo.DeleteShoppingList(shoppingListId);
        }

        public decimal CalculateTotalPrice(int shoppingListId)
        {
            var productsWithQuantities = _productListRepo.GetProductsByShoppingListId(shoppingListId);

            return productsWithQuantities.Sum(p => p.Product.Price * p.Quantity);
        }

    }
}
