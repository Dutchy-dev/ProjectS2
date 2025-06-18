using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.DataAccess.DataAcces_Exceptions;
using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using MySql.Data.MySqlClient;

namespace ClassLibrary.DataAccess.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly string _connectionString;

        public ProductRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /*
         * deze code wordt niet gebruikt in de applicatie en is een restand van het testen van de database
         */

        public Product GetById(int id)
        {
            try
            {
                Product product = null;

                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = "SELECT * FROM Product WHERE Id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    (
                        reader.GetInt32("Id"),
                        reader.GetString("Name"),
                        reader.GetString("Store"),
                        reader.GetDecimal("Price"),
                        reader.GetString("Category")
                    );
                }

                return product;

            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"ophalen van product met id {id}");
                throw;
            }
        }
    }
}
