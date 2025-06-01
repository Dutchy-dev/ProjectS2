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
    public class ProductListService
    {
        private readonly IProductListRepo _productListRepository;

        public ProductListService(IProductListRepo productListRepository)
        {
            _productListRepository = productListRepository;
        }

        public void AddProductToList(int shoppingListId, int productId, int quantity)
        {
            var item = new ProductList
            {
                ShoppingListId = shoppingListId,
                ProductId = productId,
                Quantity = quantity
            };
            _productListRepository.AddProductToList(item);
        }

        public void RemoveProductFromList(int shoppingListId, int productId)
        {
            _productListRepository.RemoveProductFromList(shoppingListId, productId);
        }

        public void ChangeQuantity(int shoppingListId, int productId, int delta)
        {
            _productListRepository.UpdateQuantity(shoppingListId, productId, delta);
        }

    }
}
*/
