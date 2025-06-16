using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;

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
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("INSERT INTO ShoppingList (Theme, User_id) VALUES (@theme, @userId)", conn);
            cmd.Parameters.AddWithValue("@theme", list.Theme);
            cmd.Parameters.AddWithValue("@userId", list.UserId);
            cmd.ExecuteNonQuery();
        }

        public List<ShoppingList> GetShoppingListsByUserId(int userId)
        {
            var result = new List<ShoppingList>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"
            SELECT Id, Theme, User_id
            FROM ShoppingList
            WHERE User_id = @User_id;
        ";

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

        public ShoppingList GetShoppingListsById(int shoppingListId)
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

            throw new Exception("ShoppingList niet gevonden.");
        }


        public void DeleteShoppingList(int shoppingListId)
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

    }
}
