using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Domain_Exceptions;

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
            try
            {
                return _productListRepo.GetProductsByShoppingListId(shoppingListId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"GetShoppingListDetails({shoppingListId})");
                throw;
            }
        }

        public ShoppingList GetShoppingListById(int shoppingListId)
        {
            try
            {
                return _shoppingListRepo.GetShoppingListById(shoppingListId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"GetShoppingListById({shoppingListId})");
                throw;
            }
        }

        public List<(ShoppingList shoppingList, List<ProductWithQuantity> products)> GetShoppingListsWithProductsByUser(int userId)
        {
            try
            {
                var shoppingLists = _shoppingListRepo.GetShoppingListsByUserId(userId);
                var result = new List<(ShoppingList, List<ProductWithQuantity>)>();

                foreach (var list in shoppingLists)
                {
                    var productsWithQuantities = _productListRepo.GetProductsByShoppingListId(list.Id);
                    result.Add((list, productsWithQuantities));
                }

                return result;
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"GetShoppingListsWithProductsByUser({userId})");
                throw;
            }
        }

        public void CreateShoppingList(string theme, int userId)
        {
            try
            {
                var list = new ShoppingList(theme, userId);
                _shoppingListRepo.Add(list);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"CreateShoppingList(theme: {theme}, userId: {userId})");
            }
        }

        public void DeleteList(int shoppingListId)
        {
            try
            {
                _shoppingListRepo.DeleteShoppingList(shoppingListId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"DeleteList({shoppingListId})");
            }
        }

        public decimal CalculateTotalPrice(int shoppingListId)
        {
            try
            {
                var productsWithQuantities = _productListRepo.GetProductsByShoppingListId(shoppingListId);
                return productsWithQuantities.Sum(p => p.Product.Price * p.Quantity);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"CalculateTotalPrice({shoppingListId})");
                return 0;
            }
        }

    }
}
