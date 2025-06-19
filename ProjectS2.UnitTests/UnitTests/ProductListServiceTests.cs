using Moq;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Domain_Exceptions;
using ClassLibrary.Domain.Interfaces;
using MySql.Data.MySqlClient;
using ClassLibrary.Domain.Services;
using ClassLibrary.DataAccess.Exceptions;

namespace ProjectS2.Tests.UnitTests
{
    [TestClass]
    public class ProductListServiceTests
    {
        private Mock<IProductListRepo> _mockRepo;
        private ProductListService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IProductListRepo>();
            _service = new ProductListService(_mockRepo.Object);
        }

        [TestMethod]
        // Test van normale werking / samenwerking tussen service en repo.
        public void AddProductToList_ValidItem_CallsRepositoryAdd()
        {
            // Arrange
            var item = new ProductList(1, 2, 3);

            // Act
            _service.AddProductToList(item);

            // Assert
            _mockRepo.Verify(r => r.AddProductToList(item), Times.Once);
        }

        [TestMethod]
        // Test van foutafhandeling en juiste exception-mapping.
        [ExpectedException(typeof(ServicesException))]
        public void AddProductToList_RepositoryException_ThrowsServicesException()
        {
            // Arrange
            var item = new ProductList(1, 2, 3);
            var repoException = new RepositoryException("Simulated repository error", null);

            _mockRepo
                .Setup(r => r.AddProductToList(It.IsAny<ProductList>()))
                .Throws(repoException);

            // Act
            _service.AddProductToList(item);

            // Assert wordt afgehandeld via [ExpectedException]
        }

        [TestMethod]
        // Test van foutafhandeling en juiste exception-mapping.
        [ExpectedException(typeof(ServicesException))]
        public void AddProductToList_MySqlException_ThrowsServicesException()
        {
            // Arrange
            var item = new ProductList(1, 2, 3);

            var sqlException = (MySqlException)Activator.CreateInstance(
                typeof(MySqlException),
                nonPublic: true
            );

            _mockRepo
                .Setup(r => r.AddProductToList(It.IsAny<ProductList>()))
                .Throws(sqlException);

            // Act
            _service.AddProductToList(item);

            // Assert wordt afgehandeld via [ExpectedException]
        }

        [TestMethod]
        // Test van foutafhandeling en juiste exception-mapping.
        [ExpectedException(typeof(ServicesException))]
        public void AddProductToList_UnexpectedException_ThrowsServicesException()
        {
            // Arrange
            var item = new ProductList(1, 2, 3);
            var unexpectedEx = new NullReferenceException("Unexpected error");

            _mockRepo
                .Setup(r => r.AddProductToList(It.IsAny<ProductList>()))
                .Throws(unexpectedEx);

            // Act
            _service.AddProductToList(item);

            // Assert wordt afgehandeld via [ExpectedException]
        }
    }
}

