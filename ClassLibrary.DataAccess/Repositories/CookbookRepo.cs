using ClassLibrary.DataAccess.DataAcces_Exceptions;
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
            try
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
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"ophalen van kookboeken voor userId {userId}");
                throw;
            }
        }

        public void CreateCookbook(Cookbook cookbook)
        {
            try
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
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"aanmaken van cookbook (Name: {cookbook.Name}, UserId: {cookbook.UserId})");
                throw;
            }
        }

        public Cookbook? GetCookbookById(int id)
        {
            try
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
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"ophalen van cookbook met id {id}");
                throw;
            }
        }

        public void UpdateCookbook(Cookbook cookbook)
        {
            try
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
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"bijwerken van cookbook (Id: {cookbook.Id}, Name: {cookbook.Name})");
                throw;
            }
        }


        public void DeleteCookbook(int id)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                var getRecipeIdsCmd = new MySqlCommand("SELECT Id FROM Recipe WHERE Cookbook_id = @cookbookId", conn);
                getRecipeIdsCmd.Parameters.AddWithValue("@cookbookId", id);

                var recipeIds = new List<int>();
                using (var reader = getRecipeIdsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        recipeIds.Add(reader.GetInt32("Id"));
                    }
                }

                foreach (var recipeId in recipeIds)
                {
                    //verwijderen van Recipe ingredient in RecipeProductList
                    using var deleteProductRecipeCmd = new MySqlCommand("DELETE FROM ProductRecipelist WHERE Recipe_id = @recipeId", conn);
                    deleteProductRecipeCmd.Parameters.AddWithValue("@recipeId", recipeId);
                    deleteProductRecipeCmd.ExecuteNonQuery();
                }


                //verwijderen van alle Recipe in Cookbook
                using var deleteRecipeCmd = new MySqlCommand("DELETE FROM Recipe WHERE Cookbook_id = @id", conn);
                deleteRecipeCmd.Parameters.AddWithValue("@id", id);
                deleteRecipeCmd.ExecuteNonQuery();


                //verwijderen van CookBook
                using var deleteCookbookCmd = new MySqlCommand("DELETE FROM Cookbook WHERE Id = @id", conn);
                deleteCookbookCmd.Parameters.AddWithValue("@id", id);
                deleteCookbookCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RepositoryExceptionHelper.HandleException(ex, $"verwijderen van cookbook met id {id}");
                throw;
            }
        }
    }
}
