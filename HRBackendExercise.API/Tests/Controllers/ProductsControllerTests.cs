using HRBackendExercise.API.Abstractions;
using HRBackendExercise.API.Controllers;
using HRBackendExercise.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HRBackendExercise.API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        #region GetAll Tests

        [Fact]
        public void GetAll_WithProducts_ReturnsOkWithProducts()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var products = new List<Product>
            {
                new Product { Id = 1, SKU = "PROD-001", Description = "Product 1", Price = 10m },
                new Product { Id = 2, SKU = "PROD-002", Description = "Product 2", Price = 20m }
            };
            mockService.Setup(s => s.GetAll()).Returns(products);
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public void GetAll_WithEmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.GetAll()).Returns(new List<Product>());
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Empty(returnedProducts);
        }

        [Fact]
        public void GetAll_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.GetAll()).Throws(new Exception("Database error"));
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion

        #region Get Tests

        [Fact]
        public void Get_WithExistingId_ReturnsOkWithProduct()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var product = new Product { Id = 1, SKU = "PROD-001", Description = "Test Product", Price = 99.99m };
            mockService.Setup(s => s.GetById(1)).Returns(product);
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(1, returnedProduct.Id);
            Assert.Equal("PROD-001", returnedProduct.SKU);
        }

        [Fact]
        public void Get_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.GetById(999)).Returns((Product?)null);
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Get(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public void Get_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.GetById(It.IsAny<int>())).Throws(new Exception("Database error"));
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion

        #region Post Tests

        [Fact]
        public void Post_WithValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var product = new Product { SKU = "PROD-001", Description = "Test Product", Price = 99.99m };
            var createdProduct = new Product { Id = 1, SKU = "PROD-001", Description = "Test Product", Price = 99.99m };
            mockService.Setup(s => s.Create(It.IsAny<Product>())).Returns(createdProduct);
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Post(product);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ProductsController.Get), createdResult.ActionName);
            var returnedProduct = Assert.IsType<Product>(createdResult.Value);
            Assert.Equal(1, returnedProduct.Id);
        }

        [Fact]
        public void Post_WithNullProduct_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Post(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Post_WithInvalidSKU_ReturnsBadRequest(string? sku)
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = sku, Description = "Test", Price = 10m };

            // Act
            var result = controller.Post(product);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99.99)]
        public void Post_WithInvalidPrice_ReturnsBadRequest(decimal price)
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Description = "Test", Price = price };

            // Act
            var result = controller.Post(product);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Post_WhenArgumentExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.Create(It.IsAny<Product>()))
                .Throws(new ArgumentException("Invalid argument"));
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Price = 10m };

            // Act
            var result = controller.Post(product);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Post_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.Create(It.IsAny<Product>()))
                .Throws(new Exception("Database error"));
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Price = 10m };

            // Act
            var result = controller.Post(product);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion

        #region Put Tests

        [Fact]
        public void Put_WithValidProduct_ReturnsNoContent()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.Update(It.IsAny<Product>()));
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Description = "Updated", Price = 99.99m };

            // Act
            var result = controller.Put(1, product);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockService.Verify(s => s.Update(It.Is<Product>(p => p.Id == 1)), Times.Once);
        }

        [Fact]
        public void Put_WithNullProduct_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Put(1, null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Put_WithInvalidSKU_ReturnsBadRequest(string? sku)
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = sku, Price = 10m };

            // Act
            var result = controller.Put(1, product);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99.99)]
        public void Put_WithInvalidPrice_ReturnsBadRequest(decimal price)
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Price = price };

            // Act
            var result = controller.Put(1, product);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Put_WithNonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.Update(It.IsAny<Product>()))
                .Throws(new InvalidOperationException("Product not found"));
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Price = 10m };

            // Act
            var result = controller.Put(999, product);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public void Put_WhenArgumentExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.Update(It.IsAny<Product>()))
                .Throws(new ArgumentException("Invalid argument"));
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Price = 10m };

            // Act
            var result = controller.Put(1, product);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Put_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.Update(It.IsAny<Product>()))
                .Throws(new Exception("Database error"));
            var controller = new ProductsController(mockService.Object);
            var product = new Product { SKU = "PROD-001", Price = 10m };

            // Act
            var result = controller.Put(1, product);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var product = new Product { Id = 1, SKU = "PROD-001", Price = 10m };
            mockService.Setup(s => s.GetById(1)).Returns(product);
            mockService.Setup(s => s.Delete(It.IsAny<Product>()));
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockService.Verify(s => s.Delete(It.Is<Product>(p => p.Id == 1)), Times.Once);
        }

        [Fact]
        public void Delete_WithNonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.GetById(999)).Returns((Product?)null);
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Delete(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public void Delete_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var product = new Product { Id = 1, SKU = "PROD-001", Price = 10m };
            mockService.Setup(s => s.GetById(1)).Returns(product);
            mockService.Setup(s => s.Delete(It.IsAny<Product>()))
                .Throws(new Exception("Database error"));
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = controller.Delete(1);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion
    }
}
