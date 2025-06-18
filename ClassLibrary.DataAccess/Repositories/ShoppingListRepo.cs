using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;
using ClassLibrary.DataAccess.DataAcces_Exceptions;

namespace ClassLibrary.DataAccess.Repositories
{
    public class ShoppingListRepo : IShoppingListRepo
    {
        private readonly string _connectionString;

        public ShoppingListRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(ShoppingList list)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                using var cmd = new MySqlCommand("INSERT INTO ShoppingList (Theme, User_id) VALUES (@theme, @userId)", conn);
                cmd.Parameters.AddWithValue("@theme", list.Theme);
                cmd.Parameters.AddWithValue("@userId", list.UserId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, "Shoppinglist toevoegen");
            }
        }

        public List<ShoppingList> GetShoppingListsByUserId(int userId)
        {
            try
            {
                var result = new List<ShoppingList>();

                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"
                    SELECT Id, Theme, User_id
                    FROM ShoppingList
                    WHERE User_id = @User_id;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", userId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var shoppingList = new ShoppingList
                    (
                        reader.GetInt32("Id"),
                        reader.GetString("Theme"),
                        reader.GetInt32("User_id")
                    );

                    result.Add(shoppingList);
                }

                return result;

            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, "Shoppinglists ophalen met user ID");
                return null;
            }
        }

        public ShoppingList GetShoppingListById(int shoppingListId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"
                SELECT Id, Theme, User_id 
                FROM ShoppingList 
                WHERE Id = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", shoppingListId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new ShoppingList(
                        reader.GetInt32("Id"),
                        reader.GetString("Theme"),
                        reader.GetInt32("User_id")
                    );
                }

                return null;
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, "Shoppinglist ophalen met ID");
                return null;
            }
        }

        public void DeleteShoppingList(int shoppingListId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                // Verwijder eerst gekoppelde producten
                using var deleteProductsCmd = new MySqlCommand("DELETE FROM ProductList WHERE ShoppingList_id = @id", connection);
                deleteProductsCmd.Parameters.AddWithValue("@id", shoppingListId);
                deleteProductsCmd.ExecuteNonQuery();

                // Verwijder daarna de lijst
                using var deleteListCmd = new MySqlCommand("DELETE FROM ShoppingList WHERE Id = @id", connection);
                deleteListCmd.Parameters.AddWithValue("@id", shoppingListId);
                deleteListCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, "Shoppinglist verwijderen");
            }
        }

        public void SaveProductListItem(int shoppingListId, int productId, int quantity)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                // Bestaat het item al?
                var exists = GetProductQuantity(shoppingListId, productId) > 0;

                if (exists)
                {
                    // UPDATE
                    using var updateCmd = new MySqlCommand(@"
                        UPDATE ProductList
                        SET Quantity = @quantity
                        WHERE ShoppingList_id = @listId AND Product_id = @productId", conn);

                    updateCmd.Parameters.AddWithValue("@quantity", quantity);
                    updateCmd.Parameters.AddWithValue("@listId", shoppingListId);
                    updateCmd.Parameters.AddWithValue("@productId", productId);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    // INSERT
                    using var insertCmd = new MySqlCommand(@"
                        INSERT INTO ProductList (ShoppingList_id, Product_id, Quantity)
                        VALUES (@listId, @productId, @quantity)", conn);

                    insertCmd.Parameters.AddWithValue("@listId", shoppingListId);
                    insertCmd.Parameters.AddWithValue("@productId", productId);
                    insertCmd.Parameters.AddWithValue("@quantity", quantity);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, "Product opslaan naar shoppinglist");
            }
        }

        public int GetProductQuantity(int shoppingListId, int productId)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                using var cmd = new MySqlCommand(@"
                    SELECT Quantity
                    FROM ProductList
                    WHERE ShoppingList_id = @listId AND Product_id = @productId", conn);

                cmd.Parameters.AddWithValue("@listId", shoppingListId);
                cmd.Parameters.AddWithValue("@productId", productId);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, "Haal product quantity op");
                return 0;
            }
        }
    }
}
