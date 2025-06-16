using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DataAccess.Repositories
{
    public class CookbookRepo : ICookbookRepo
    {
        private readonly string _connectionString;

        public CookbookRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Cookbook> GetCookbooksByUserId(int userId)
        {
            List<Cookbook> cookbooks = new();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "SELECT Id, Name, User_id, Description FROM Cookbook WHERE User_id = @userId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cookbooks.Add(new Cookbook(
                    id: reader.GetInt32("Id"),
                    userId: reader.GetInt32("User_id"),
                    name: reader.GetString("Name"),
                    description: reader.IsDBNull("Description") ? null : reader.GetString("Description")
                ));
            }

            return cookbooks;
        }
        public void CreateCookbook(Cookbook cookbook)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "INSERT INTO Cookbook (Name, User_id, Description) VALUES (@name, @userId, @description)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", cookbook.Name);
            cmd.Parameters.AddWithValue("@userId", cookbook.UserId);
            cmd.Parameters.AddWithValue("@description", (object?)cookbook.Description ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public Cookbook? GetCookbookById(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "SELECT Id, Name, User_id, Description FROM Cookbook WHERE Id = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Cookbook(
                    id: reader.GetInt32("Id"),
                    userId: reader.GetInt32("User_id"),
                    name: reader.GetString("Name"),
                    description: reader.IsDBNull("Description") ? null : reader.GetString("Description")
                );
            }

            return null;
        }

        public void UpdateCookbook(Cookbook cookbook)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "UPDATE Cookbook SET Name = @name, Description = @description WHERE Id = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", cookbook.Name);
            cmd.Parameters.AddWithValue("@id", cookbook.Id);
            cmd.Parameters.AddWithValue("@description", (object?)cookbook.Description ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void DeleteCookbook(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "DELETE FROM Cookbook WHERE Id = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

    }
}
