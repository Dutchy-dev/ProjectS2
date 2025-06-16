using ClassLibrary.DataAccess.Repositories;
using ClassLibrary.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace ProjectS2.Tests.IntegrationTests
{
    public static class TestConfiguration
    {
        public static IConfigurationRoot Load()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Zorg dat je testproject de juiste root heeft
                .AddJsonFile("appsettings.Test.json")
                .Build();
        }
    }

    [TestClass]
    public class ProductListRepoIntegrationTests
    {
        private ProductListRepo _repo;
        private string _connectionString;
        private const int _testUserId = 1; // vaste user die al bestaat
        private const int _testProductId = 1; // vast product dat al bestaat
        private int _testShoppingListId; // dynamisch aangemaakt in setup

        [TestInitialize]
        public void Setup()
        {
            var config = TestConfiguration.Load();
            _connectionString = config.GetConnectionString("TestDatabase");
            _repo = new ProductListRepo(_connectionString);

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            // Maak test shoppinglist aan
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO shoppinglist (Theme, User_id) VALUES ('TestList', @userId); SELECT LAST_INSERT_ID();";
            cmd.Parameters.AddWithValue("@userId", _testUserId);
            _testShoppingListId = Convert.ToInt32(cmd.ExecuteScalar());
        }

        [TestMethod]
        public void AddProductToList_And_GetProductsByShoppingListId_WorksCorrectly()
        {
            // Arrange
            int quantity = 3;
            var item = new ProductList(_testShoppingListId, _testProductId, quantity);

            // Act
            _repo.AddProductToList(item);
            var results = _repo.GetProductsByShoppingListId(_testShoppingListId);

            // Assert
            var match = results.FirstOrDefault(p => p.Product.Id == _testProductId && p.Quantity == quantity);
            Assert.IsNotNull(match, "Product werd niet teruggevonden in de database.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                // Verwijder het product uit de lijst via repository
                try { _repo.RemoveProductFromList(_testShoppingListId, _testProductId); }
                catch (Exception ex)
                {
                    Console.WriteLine("Cleanup failed for product removal: " + ex.Message);
                }

                // Verwijder ook de shoppinglist en user direct via MySQL
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                // Eerst de shoppinglist verwijderen (alle verwijzingen moeten weg zijn)
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM shoppinglist WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", _testShoppingListId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cleanup failed: " + ex.Message);
            }
        }
    }
}
