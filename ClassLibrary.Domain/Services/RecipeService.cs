using ClassLibrary.Domain.Domain_Exceptions;
using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Services
{
    public class RecipeService
    {
        private readonly IRecipeRepo _recipeRepo;
        private readonly IShoppingListRepo _shoppingListRepo;

        public RecipeService(IRecipeRepo recipeRepo, IShoppingListRepo shoppingListRepo)
        {
            _recipeRepo = recipeRepo;
            _shoppingListRepo = shoppingListRepo;
        }

        public List<Recipe> GetAllRecipesForCookbook(int cookbookId)
        {
            try
            {
                return _recipeRepo.GetAllRecipesForCookbook(cookbookId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Alle recepten ophalen voor kookboek {cookbookId}");
                throw;
            }
        }

        public Recipe? GetRecipeById(int id)
        {
            try
            {
                var recipe = _recipeRepo.GetRecipeById(id);
                if (recipe == null)
                    throw new ServicesException($"Recept met ID {id} niet gevonden.", null!);

                var products = _recipeRepo.GetProductsForRecipe(id);
                recipe.Products.AddRange(products);
                return recipe;
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Recept ophalen met ID {id}");
                throw;
            }
        }

        public List<ProductWithQuantity> GetProductsForRecipe(int recipeId)
        {
            try
            {
                return _recipeRepo.GetProductsForRecipe(recipeId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Producten ophalen voor recept {recipeId}");
                throw;
            }
        }

        public bool UpdateRecipe(int recipeId, string name, string description)
        {
            try
            {
                return _recipeRepo.UpdateRecipe(recipeId, name, description);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Recept bijwerken (ID: {recipeId})");
                throw;
            }
        }

        public bool UpdateProductQuantity(int recipeId, int productId, int quantity)
        {
            try
            {
                return _recipeRepo.UpdateProductQuantity(recipeId, productId, quantity);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Producthoeveelheid bijwerken (Recept: {recipeId}, Product: {productId})");
                throw;
            }
        }

        public void Create(Recipe recipe)
        {
            try
            {
                _recipeRepo.Create(recipe);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Recept aanmaken (Naam: {recipe.Name})");
            }
        }

        public void DeleteRecipe(int id)
        {
            try
            {
                _recipeRepo.DeleteRecipe(id);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Recept verwijderen (ID: {id})");
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                return _recipeRepo.GetAllProducts();
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "Alle producten ophalen");
                throw;
            }
        }

        public void AddProductToRecipe(RecipeProduct item)
        {
            if (item.Quantity < 1)
            {
                Log.Warning("Quantity mag niet kleiner zijn dan 1 (RecipeId: {RecipeId}, ProductId: {ProductId})",
                    item.RecipeId, item.ProductId);
                throw new ValidationException("Aantal moet minimaal 1 zijn.");
            }

            try
            {
                _recipeRepo.AddProductToRecipe(item.RecipeId, item.ProductId, item.Quantity);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "Product toevoegen aan recept");
            }
        }

        public void RemoveProductFromRecipe(int recipeId, int productId)
        {
            try
            {
                _recipeRepo.RemoveProductFromRecipe(recipeId, productId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Product verwijderen uit recept (Recept: {recipeId}, Product: {productId})");
            }
        }

        public List<Product> GetFilteredProducts(ProductFilter filter)
        {
            try
            {
                return _recipeRepo.GetFilteredProducts(filter);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, "Gefilterde producten ophalen");
                throw;
            }
        }

        public void AddRecipeToShoppingList(int recipeId, int shoppingListId)
        {
            try
            {
                var recipeWithoutProducts = _recipeRepo.GetRecipeById(recipeId)
                    ?? throw new ServicesException($"Recept met ID {recipeId} niet gevonden.", null!);

                var products = _recipeRepo.GetProductsForRecipe(recipeId);

                var recipe = new Recipe(
                    recipeWithoutProducts.Id,
                    recipeWithoutProducts.Name,
                    recipeWithoutProducts.Description,
                    recipeWithoutProducts.CookbookId,
                    products
                );

                foreach (var productWithQuantity in recipe.Products)
                {
                    int existingQuantity = _shoppingListRepo.GetProductQuantity(shoppingListId, productWithQuantity.Product.Id);
                    int newQuantity = existingQuantity + productWithQuantity.Quantity;

                    _shoppingListRepo.SaveProductListItem(
                        shoppingListId,
                        productWithQuantity.Product.Id,
                        newQuantity
                    );
                }
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Recept {recipeId} toevoegen aan boodschappenlijst {shoppingListId}");
            }
        }
    }
}
