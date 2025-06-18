using ClassLibrary.DataAccess.DataAcces_Exceptions;
using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using MySql.Data.MySqlClient;
using System.Text;

namespace ClassLibrary.DataAccess.Repositories
{
    public class ProductListRepo : IProductListRepo
    {
        private readonly string _connectionString;

        public ProductListRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductWithQuantity> GetProductsByShoppingListId(int shoppingListId)
        {
            try
            {
                var result = new List<ProductWithQuantity>();

                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"
                    SELECT p.Id, p.Name, p.Store, p.Price, p.Category, pl.Quantity
                    FROM ProductList pl
                    JOIN Product p ON pl.Product_id = p.Id
                    WHERE pl.ShoppingList_id = @shoppingListId;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@shoppingListId", shoppingListId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product
                    (
                        reader.GetInt32("Id"),
                        reader.GetString("Name"),
                        reader.GetString("Store"),
                        reader.GetDecimal("Price"),
                        reader.GetString("Category")
                    );

                    int quantity = reader.GetInt32("Quantity");
                    result.Add(new ProductWithQuantity(product, quantity));
                }

                return result;

            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"ophalen producten voor shoppinglist (Id: {shoppingListId})");
                return null!;
            }
        }      

        public void AddProductToList(ProductList item)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                using var cmd = new MySqlCommand("INSERT INTO ProductList (ShoppingList_id, Product_id, Quantity) VALUES (@listId, @productId, @quantity)", connection);
                cmd.Parameters.AddWithValue("@listId", item.ShoppingListId);
                cmd.Parameters.AddWithValue("@productId", item.ProductId);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"toevoegen product aan lijst (ListId: {item.ShoppingListId}, ProductId: {item.ProductId})");
            }
        }

        public void RemoveProductFromList(int shoppingListId, int productId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM ProductList WHERE ShoppingList_id = @listId AND Product_id = @productId";
                cmd.Parameters.AddWithValue("@listId", shoppingListId);
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"verwijderen product uit lijst (ListId: {shoppingListId}, ProductId: {productId})");
            }
        }

        public void UpdateQuantity(int shoppingListId, int productId, int delta)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                // Haal huidige hoeveelheid op
                using var getCmd = new MySqlCommand(
                    "SELECT Quantity FROM ProductList WHERE ShoppingList_id = @listId AND Product_id = @productId", connection);
                getCmd.Parameters.AddWithValue("@listId", shoppingListId);
                getCmd.Parameters.AddWithValue("@productId", productId);

                var result = getCmd.ExecuteScalar();

                if (result != null)
                {
                    int currentQuantity = Convert.ToInt32(result);
                    int newQuantity = currentQuantity + delta;

                    // Zorg dat quantity minimaal 1 blijft
                    if (newQuantity < 1)
                        newQuantity = 1;

                    using var updateCmd = new MySqlCommand(
                        "UPDATE ProductList SET Quantity = @quantity WHERE ShoppingList_id = @listId AND Product_id = @productId", connection);
                    updateCmd.Parameters.AddWithValue("@quantity", newQuantity);
                    updateCmd.Parameters.AddWithValue("@listId", shoppingListId);
                    updateCmd.Parameters.AddWithValue("@productId", productId);

                    updateCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"bijwerken hoeveelheid (ListId: {shoppingListId}, ProductId: {productId}, Delta: {delta})");
            }
        }

        public List<Product> GetFilteredProducts(ProductFilter filter)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                return ProductQueryHelper.ExecuteFilteredProductQuery(connection, filter);
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, "filteren van producten");
                return new List<Product>();
            }
        }

    }
}