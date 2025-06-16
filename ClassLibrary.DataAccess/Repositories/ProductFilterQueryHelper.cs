using ClassLibrary.Domain.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DataAccess.Repositories
{
    public static class ProductQueryHelper
    {
        public static List<Product> ExecuteFilteredProductQuery(MySqlConnection connection, ProductFilter filter)
        {
            var products = new List<Product>();

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
                products.Add(new Product
                (
                    reader.GetInt32("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Store"),
                    reader.GetDecimal("Price"),
                    reader.GetString("Category")
                ));
            }

            return products;
        }
    }

}
