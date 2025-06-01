using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;
using System.Reflection;

namespace ClassLibrary.Domain.Services
{
    public class ProductListService
    {
        private readonly IProductListRepo _productListRepository;

        public ProductListService(IProductListRepo productListRepository)
        {
            _productListRepository = productListRepository;
        }

        public void AddProductToList(ProductList item)
        { 
            _productListRepository.AddProductToList(item);
        }

        public List<Product> GetFilteredProducts(ProductFilter filter)
        {
            return _productListRepository.GetFilteredProducts(filter);
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
