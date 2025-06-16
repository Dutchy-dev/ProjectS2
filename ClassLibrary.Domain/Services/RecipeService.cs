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

        public RecipeService(IRecipeRepo recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

        public List<Recipe> GetAllRecipesForCookbook(int cookbookId)
        {
            return _recipeRepo.GetAllRecipesForCookbook(cookbookId);
        }

        public Recipe? GetRecipeById(int id)
        {
            var recipe = _recipeRepo.GetRecipeById(id);
            var products = _recipeRepo.GetProductsForRecipe(id);
            recipe.Products.AddRange(products); 
            return recipe;
        }

        public List<ProductWithQuantity> GetProductsForRecipe(int recipeId)
        {
            return _recipeRepo.GetProductsForRecipe(recipeId);
        }

        public bool UpdateRecipe(int recipeId, string name, string description)
        {
            return _recipeRepo.UpdateRecipe(recipeId, name, description);
        }

        public bool UpdateProductQuantity(int recipeId, int productId, int quantity)
        {
            return _recipeRepo.UpdateProductQuantity(recipeId, productId, quantity);
        }

        public void Create(Recipe recipe)
        {
            _recipeRepo.Create(recipe);
        }

        public void DeleteRecipe(int id)
        {
            _recipeRepo.DeleteRecipe(id);
        }

        public List<Product> GetAllProducts()
        {
            return _recipeRepo.GetAllProducts();
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
                ServiceExceptionHelper.HandleException(ex, "toevoegen product aan recept");
            }
        }

        public void RemoveProductFromRecipe(int recipeId, int productId)
        {
            _recipeRepo.RemoveProductFromRecipe(recipeId, productId);
        }

        public List<Product> GetFilteredProducts(ProductFilter filter)
        {
            return _recipeRepo.GetFilteredProducts(filter);
        }
    }
}
