using ClassLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Interfaces
{
    public interface IRecipeRepo
    {
        List<Recipe> GetAllRecipesForCookbook(int cookbookId);

        Recipe? GetRecipeById(int id);

        List<ProductWithQuantity> GetProductsForRecipe(int recipeId);

        bool UpdateRecipe(int recipeId, string name, string description);

        bool UpdateProductQuantity(int recipeId, int productId, int quantity);

        void Create(Recipe recipe);

        void DeleteRecipe(int id);

        List<Product> GetAllProducts();

        void AddProductToRecipe(int recipeId, int productId, int quantity);

        void RemoveProductFromRecipe(int recipeId, int productId);

        List<Product> GetFilteredProducts(ProductFilter filter);

    }
}
