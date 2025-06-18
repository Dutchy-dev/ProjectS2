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
        private readonly IProductListRepo _productListRepo;

        public ProductListService(IProductListRepo productListRepository)
        {
            _productListRepo = productListRepository;
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
                _productListRepo.AddProductToList(item);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "toevoegen product aan lijst");
            }
        }

        public List<Product> GetFilteredProducts(ProductFilter filter)
        {
            try
            {
                return _productListRepo.GetFilteredProducts(filter);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "filteren producten");
                return new List<Product>(); // Return an empty list in case of an error
            }
        }

        public void RemoveProductFromList(int shoppingListId, int productId)
        {
            if (shoppingListId <= 0 || productId <= 0)
            {
                Log.Warning("Ongeldige invoer: ShoppingListId en ProductId moeten groter zijn dan 0 (ShoppingListId: {ListId}, ProductId: {ProductId})",
                    shoppingListId, productId);
                throw new ValidationException("ShoppingListId en ProductId moeten groter zijn dan 0.");
            }

            try
            {
                _productListRepo.RemoveProductFromList(shoppingListId, productId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "verwijderen product uit lijst");
            }
        }

        public void ChangeQuantity(int shoppingListId, int productId, int delta)
        {
            if (delta == 0)
            {
                Log.Warning("Ongeldige delta: 0. Geen wijziging in hoeveelheid. (ShoppingListId: {ListId}, ProductId: {ProductId})",
                    shoppingListId, productId);
                throw new ValidationException("Aantalwijziging mag niet 0 zijn.");
            }

            try
            {
                _productListRepo.UpdateQuantity(shoppingListId, productId, delta);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "hoeveelheid wijzigen");
            }
        }

    }
}
