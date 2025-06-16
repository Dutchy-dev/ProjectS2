using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DataAccess.Repositories
{
    public class RecipeRepo : IRecipeRepo
    {
        private readonly string _connectionString;

        public RecipeRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Recipe> GetAllRecipesForCookbook(int cookbookId)
        {
            List<Recipe> recipes = new();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, Name, Description, Cookbook_id FROM Recipe WHERE Cookbook_id = @cookbookId";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@cookbookId", cookbookId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                recipes.Add(new Recipe(
                    reader.GetInt32("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Description"),
                    reader.GetInt32("Cookbook_id")
                ));
            }

            return recipes;
        }

        public Recipe? GetRecipeById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, Name, Description, Cookbook_id FROM Recipe WHERE Id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Recipe(
                    reader.GetInt32("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Description"),
                    reader.GetInt32("Cookbook_id")
                );
            }

            return null;
        }

        public List<ProductWithQuantity> GetProductsForRecipe(int recipeId)
        {
            var result = new List<ProductWithQuantity>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"
            SELECT p.Id, p.Name, p.Category, p.Price, p.Store, prl.Quantity
            FROM ProductRecipeList prl
            INNER JOIN Product p ON p.Id = prl.Product_id
            WHERE prl.Recipe_id = @recipeId";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@recipeId", recipeId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var product = new Product(
                    reader.GetInt32("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Category"),
                    reader.GetDecimal("Price"),
                    reader.GetString("Store")
                );

                var quantity = reader.GetInt32("Quantity");
                result.Add(new ProductWithQuantity(product, quantity));
            }

            return result;
        }

        public bool UpdateRecipe(int recipeId, string name, string description)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = "UPDATE Recipe SET Name = @name, Description = @description WHERE Id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@id", recipeId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateProductQuantity(int recipeId, int productId, int quantity)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = "UPDATE ProductRecipeList SET Quantity = @quantity WHERE Recipe_id = @recipeId AND Product_id = @productId";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@recipeId", recipeId);
            cmd.Parameters.AddWithValue("@productId", productId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public void Create(Recipe recipe)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "INSERT INTO Recipe (Name, Description, Cookbook_id) VALUES (@name, @description, @cookbookId)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", recipe.Name);
            cmd.Parameters.AddWithValue("@description", recipe.Description);
            cmd.Parameters.AddWithValue("@cookbookId", recipe.CookbookId);

            cmd.ExecuteNonQuery();
        }

        public void DeleteRecipe(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM Recipe WHERE Id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public List<Product> GetAllProducts()
        {
            var result = new List<Product>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, Name, Store, Price, Category FROM Product";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Product(
                    reader.GetInt32("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Store"),
                    reader.GetDecimal("Price"),
                    reader.GetString("Category")
                ));
            }

            return result;
        }

        public void AddProductToRecipe(int recipeId, int productId, int quantity)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO ProductRecipelist (Recipe_id, Product_id, Quantity) VALUES (@recipeId, @productId, @quantity)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@recipeId", recipeId);
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.ExecuteNonQuery();
        }

        public void RemoveProductFromRecipe(int recipeId, int productId)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM ProductRecipelist WHERE Recipe_id = @recipeId AND Product_id = @productId";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@recipeId", recipeId);
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.ExecuteNonQuery();
        }

        public List<Product> GetFilteredProducts(ProductFilter filter)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return ProductQueryHelper.ExecuteFilteredProductQuery(connection, filter);
        }
    }
}
