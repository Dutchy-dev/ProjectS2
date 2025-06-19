using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectS2.Tests.UnitTests
{
    [TestClass]
    public class ShoppingListServiceTests
    {
        private Mock<IShoppingListRepo> _mockShoppingListRepo;
        private Mock<IProductListRepo> _mockProductListRepo;
        private ShoppingListService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockShoppingListRepo = new Mock<IShoppingListRepo>();
            _mockProductListRepo = new Mock<IProductListRepo>();
            _service = new ShoppingListService(_mockShoppingListRepo.Object, _mockProductListRepo.Object);
        }

        [TestMethod]
        public void GetShoppingListDetails_ReturnsExpectedProducts()
        {
            // Arrange
            int shoppingListId = 1;

            var productsFromRepo = new List<ProductWithQuantity>
            {
                new(new Product(1, "Melk", "Ah", 5.20m, "a"), 1),
                new(new Product(2, "Brood", "Baba", 9.34m, "b"), 2)
            };

            var expected = new List<ProductWithQuantity>
            {
                new(new Product(1, "Melk", "Ah", 5.20m, "a"), 1),
                new(new Product(2, "Brood", "Baba", 9.34m, "b"), 2)
            };

            _mockProductListRepo
                .Setup(repo => repo.GetProductsByShoppingListId(shoppingListId))
                .Returns(productsFromRepo);

            // Act
            var result = _service.GetShoppingListDetails(shoppingListId);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetShoppingListById_ReturnsCorrectList()
        {
            // Arrange
            int listId = 42;

            var repoResult = new ShoppingList(listId, "Weekboodschappen", 3);

            var expected = new ShoppingList(listId, "Weekboodschappen", 3);

            _mockShoppingListRepo
                .Setup(repo => repo.GetShoppingListById(listId))
                .Returns(repoResult);

            // Act
            var result = _service.GetShoppingListById(listId);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetShoppingListsWithProductsByUser_ReturnsListsWithProducts()
        {
            // Arrange
            int userId = 5;

            var shoppingListsFromRepo = new List<ShoppingList>
            {
                new(1, "Diner", userId),
                new(2, "Lunch", userId)
            };

            var expectedShoppingLists = new List<ShoppingList>
            {
                new(1, "Diner", userId),
                new(2, "Lunch", userId)
            };

            var productsForList1 = new List<ProductWithQuantity>
            {
                new(new Product(1, "Pasta", "Ah", 3.00m, "a"), 1)
            };

            var expectedProductsForList1 = new List<ProductWithQuantity>
            {
                new(new Product(1, "Pasta", "Ah", 3.00m, "a"), 1)
            };

            var productsForList2 = new List<ProductWithQuantity>
            {
                new(new Product(2, "Kaas", "Lidl", 2.50m, "b"), 2)
            };

            var expectedProductsForList2 = new List<ProductWithQuantity>
            {
                new(new Product(2, "Kaas", "Lidl", 2.50m, "b"), 2)
            };

            _mockShoppingListRepo
                .Setup(r => r.GetShoppingListsByUserId(userId))
                .Returns(shoppingListsFromRepo);
            _mockProductListRepo
                .Setup(r => r.GetProductsByShoppingListId(1))
                .Returns(productsForList1);
            _mockProductListRepo
                .Setup(r => r.GetProductsByShoppingListId(2))
                .Returns(productsForList2);

            // Act
            var result = _service.GetShoppingListsWithProductsByUser(userId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedShoppingLists[0], result[0].Item1);
            CollectionAssert.AreEqual(expectedProductsForList1, result[0].Item2);
            Assert.AreEqual(expectedShoppingLists[1], result[1].Item1);
            CollectionAssert.AreEqual(expectedProductsForList2, result[1].Item2);
        }

        [TestMethod]
        public void CreateShoppingList_CallsRepoAdd()
        {
            // Arrange
            string theme = "Weekend";
            int userId = 7;

            // Act
            _service.CreateShoppingList(theme, userId);

            // Assert
            _mockShoppingListRepo.Verify(repo => 
                repo.Add(It.Is<ShoppingList>(s =>
                s.Theme == theme && s.UserId == userId)), Times.Once);
        }

        [TestMethod]
        public void DeleteList_CallsRepoDelete()
        {
            // Arrange
            int id = 10;

            // Act
            _service.DeleteList(id);

            // Assert
            _mockShoppingListRepo.Verify(repo => 
                repo.DeleteShoppingList(id), Times.Once);
        }

        [TestMethod]
        public void CalculateTotalPrice_ReturnsCorrectTotal()
        {
            // Arrange
            int listId = 1;
            var products = new List<ProductWithQuantity>
            {
                new(new Product(1, "Appel", "Ah", 0.50m, "a"), 4),
                new(new Product(2, "Sinaasappel", "Lidl", 0.75m, "b"), 2)
            };

            _mockProductListRepo
                .Setup(repo => repo.GetProductsByShoppingListId(listId))
                .Returns(products);

            // Act
            var result = _service.CalculateTotalPrice(listId);

            // Assert
            decimal expectedTotal = (0.50m * 4) + (0.75m * 2); // 2.00 + 1.50 = 3.50
            Assert.AreEqual(expectedTotal, result);
        }
    }
}
