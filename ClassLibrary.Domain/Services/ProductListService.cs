using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;
using System.Reflection;
using ClassLibrary.Domain.Domain_Exceptions;
using Serilog;
using System.ComponentModel.DataAnnotations;

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
            // Validatie: Quantity moet minimaal 1 zijn
            if (item.Quantity < 1)
            {
                Log.Warning("Ongeldige invoer: Quantity mag niet kleiner zijn dan 1 (ShoppingListId: {ListId}, ProductId: {ProductId}, Quantity: {Quantity})",
                    item.ShoppingListId, item.ProductId, item.Quantity);

                throw new ValidationException("Aantal moet minimaal 1 zijn.");
            }

            try
            {
                _productListRepository.AddProductToList(item);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "toevoegen product aan lijst");
            }
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
