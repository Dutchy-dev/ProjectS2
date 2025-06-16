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
    public class UserRepo : IUserRepo
    {
        private readonly string _connectionString;

        public UserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User? GetByUsername(string username)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("SELECT Id, Username, password_hash FROM User WHERE Username = @Username", conn);
            cmd.Parameters.AddWithValue("@Username", username);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32("Id");
                string name = reader.GetString("Username");
                string passwordHash = reader.GetString("password_hash");
                return new User(id, name, passwordHash);
            }
            return null;
        }

        public void Create(string username, string passwordHash)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            using var cmd = new MySqlCommand("INSERT INTO User (Username, password_hash) VALUES (@Username, @hash)", conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@hash", passwordHash);
            cmd.ExecuteNonQuery();
        }
    }
}
