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
    public class ProductListRepo : IProductListRepo
    {
        private readonly string _connectionString = "server=localhost;port=3306;database=watetenwe;user=root;password=Brompton1102XD;";
        
        public List<ProductWithQuantity> GetProductsByShoppingListId(int shoppingListId)
        {
            var result = new List<ProductWithQuantity>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT p.Id, p.Name, p.Store, p.Price, p.Category, pl.Quantity
                FROM ProductList pl
                JOIN Product p ON pl.Product_id = p.Id
                WHERE pl.ShoppingList_id = @shoppingListId;
            ";

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

        public void AddProductToList(ProductList item)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var cmd = new MySqlCommand("INSERT INTO ProductList (ShoppingList_id, Product_id, Quantity) VALUES (@listId, @productId, @quantity)", connection);
            cmd.Parameters.AddWithValue("@listId", item.ShoppingListId);
            cmd.Parameters.AddWithValue("@productId", item.ProductId);
            cmd.Parameters.AddWithValue("@quantity", item.Quantity);
            cmd.ExecuteNonQuery();
        }

        public void RemoveProductFromList(int shoppingListId, int productId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM ProductList WHERE ShoppingList_id = @listId AND Product_id = @productId";
                cmd.Parameters.AddWithValue("@listId", shoppingListId);
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateQuantity(int shoppingListId, int productId, int delta)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            // Haal huidige hoeveelheid op
            var getCmd = new MySqlCommand(
                "SELECT Quantity FROM ProductList WHERE ShoppingList_id = @listId AND Product_id = @productId",
                connection);
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

                var updateCmd = new MySqlCommand(
                    "UPDATE ProductList SET Quantity = @quantity WHERE ShoppingList_id = @listId AND Product_id = @productId",
                    connection);
                updateCmd.Parameters.AddWithValue("@quantity", newQuantity);
                updateCmd.Parameters.AddWithValue("@listId", shoppingListId);
                updateCmd.Parameters.AddWithValue("@productId", productId);

                updateCmd.ExecuteNonQuery();
            }
        }

        public List<Product> GetFilteredProducts(ProductFilter filter)
        {
            var ProductsBasedOnFilter = new List<Product>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = new StringBuilder("SELECT * FROM Product WHERE 1=1");
            if (!string.IsNullOrEmpty(filter.Name))
                query.Append(" AND Name LIKE @name");
            if (!string.IsNullOrEmpty(filter.Store))
                query.Append(" AND Store = @store");
            if (!string.IsNullOrEmpty(filter.Category))
                query.Append(" AND Category = @category");

            using var cmd = new MySqlCommand(query.ToString(), connection);

            if (!string.IsNullOrEmpty(filter.Name))
                cmd.Parameters.AddWithValue("@name", $"%{filter.Name}%");
            if (!string.IsNullOrEmpty(filter.Store))
                cmd.Parameters.AddWithValue("@store", filter.Store);
            if (!string.IsNullOrEmpty(filter.Category))
                cmd.Parameters.AddWithValue("@category", filter.Category);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProductsBasedOnFilter.Add(new Product
                (
                    reader.GetInt32("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Store"),
                    reader.GetDecimal("Price"),
                    reader.GetString("Category")
                ));
            }

            return ProductsBasedOnFilter;
        }

    }
}


