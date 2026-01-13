using HRBackendExercise.API.Models;
using HRBackendExercise.API.Services;

namespace HRBackendExercise.API.Tests.Services
{
    public class ProductServiceTests
    {
        #region Create Tests

        [Fact]
        public void Create_WithValidProduct_ReturnsProductWithId()
        {
            // Arrange
            var service = new ProductService();
            var product = new Product
            {
                SKU = "PROD-001",
                Description = "Test Product",
                Price = 99.99m
            };

            // Act
            var result = service.Create(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("PROD-001", result.SKU);
            Assert.Equal("Test Product", result.Description);
            Assert.Equal(99.99m, result.Price);
        }

        [Fact]
        public void Create_WithNullProduct_ThrowsArgumentNullException()
        {
            // Arrange
            var service = new ProductService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.Create(null!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidSKU_ThrowsArgumentException(string? sku)
        {
            // Arrange
            var service = new ProductService();
            var product = new Product
            {
                SKU = sku,
                Description = "Test Product",
                Price = 99.99m
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.Create(product));
            Assert.Contains("SKU", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99.99)]
        public void Create_WithInvalidPrice_ThrowsArgumentException(decimal price)
        {
            // Arrange
            var service = new ProductService();
            var product = new Product
            {
                SKU = "PROD-001",
                Description = "Test Product",
                Price = price
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.Create(product));
            Assert.Contains("Price", exception.Message);
        }

        [Fact]
        public void Create_MultipleProducts_GeneratesIncrementalIds()
        {
            // Arrange
            var service = new ProductService();
            var product1 = new Product { SKU = "PROD-001", Price = 10m };
            var product2 = new Product { SKU = "PROD-002", Price = 20m };
            var product3 = new Product { SKU = "PROD-003", Price = 30m };

            // Act
            var result1 = service.Create(product1);
            var result2 = service.Create(product2);
            var result3 = service.Create(product3);

            // Assert
            Assert.Equal(1, result1.Id);
            Assert.Equal(2, result2.Id);
            Assert.Equal(3, result3.Id);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_WithExistingId_ReturnsProduct()
        {
            // Arrange
            var service = new ProductService();
            var product = new Product
            {
                SKU = "PROD-001",
                Description = "Test Product",
                Price = 99.99m
            };
            var created = service.Create(product);

            // Act
            var result = service.GetById(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("PROD-001", result.SKU);
        }

        [Fact]
        public void GetById_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var result = service.GetById(999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetAll Tests

        [Fact]
        public void GetAll_WithEmptyList_ReturnsEmptyEnumerable()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var result = service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WithProducts_ReturnsAllProducts()
        {
            // Arrange
            var service = new ProductService();
            service.Create(new Product { SKU = "PROD-001", Price = 10m });
            service.Create(new Product { SKU = "PROD-002", Price = 20m });
            service.Create(new Product { SKU = "PROD-003", Price = 30m });

            // Act
            var result = service.GetAll().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_WithValidProduct_UpdatesSuccessfully()
        {
            // Arrange
            var service = new ProductService();
            var product = service.Create(new Product
            {
                SKU = "PROD-001",
                Description = "Original Description",
                Price = 99.99m
            });

            var updatedProduct = new Product
            {
                Id = product.Id,
                SKU = "PROD-001-UPDATED",
                Description = "Updated Description",
                Price = 149.99m
            };

            // Act
            service.Update(updatedProduct);

            // Assert
            var result = service.GetById(product.Id);
            Assert.NotNull(result);
            Assert.Equal("PROD-001-UPDATED", result.SKU);
            Assert.Equal("Updated Description", result.Description);
            Assert.Equal(149.99m, result.Price);
        }

        [Fact]
        public void Update_WithNullProduct_ThrowsArgumentNullException()
        {
            // Arrange
            var service = new ProductService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.Update(null!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithInvalidSKU_ThrowsArgumentException(string? sku)
        {
            // Arrange
            var service = new ProductService();
            var product = service.Create(new Product { SKU = "PROD-001", Price = 10m });

            var updatedProduct = new Product
            {
                Id = product.Id,
                SKU = sku,
                Price = 20m
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.Update(updatedProduct));
            Assert.Contains("SKU", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99.99)]
        public void Update_WithInvalidPrice_ThrowsArgumentException(decimal price)
        {
            // Arrange
            var service = new ProductService();
            var product = service.Create(new Product { SKU = "PROD-001", Price = 10m });

            var updatedProduct = new Product
            {
                Id = product.Id,
                SKU = "PROD-001",
                Price = price
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.Update(updatedProduct));
            Assert.Contains("Price", exception.Message);
        }

        [Fact]
        public void Update_WithNonExistingId_ThrowsInvalidOperationException()
        {
            // Arrange
            var service = new ProductService();
            var product = new Product
            {
                Id = 999,
                SKU = "PROD-999",
                Price = 99.99m
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => service.Update(product));
            Assert.Contains("not found", exception.Message);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_WithExistingProduct_RemovesProduct()
        {
            // Arrange
            var service = new ProductService();
            var product = service.Create(new Product { SKU = "PROD-001", Price = 10m });

            // Act
            service.Delete(product);

            // Assert
            var result = service.GetById(product.Id);
            Assert.Null(result);
        }

        [Fact]
        public void Delete_WithNullProduct_ThrowsArgumentNullException()
        {
            // Arrange
            var service = new ProductService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.Delete(null!));
        }

        [Fact]
        public void Delete_WithNonExistingProduct_DoesNotThrowException()
        {
            // Arrange
            var service = new ProductService();
            var product = new Product
            {
                Id = 999,
                SKU = "PROD-999",
                Price = 99.99m
            };

            // Act & Assert
            // Should not throw exception even if product doesn't exist
            service.Delete(product);
        }

        [Fact]
        public void Delete_MultipleProducts_RemovesOnlySpecifiedProduct()
        {
            // Arrange
            var service = new ProductService();
            var product1 = service.Create(new Product { SKU = "PROD-001", Price = 10m });
            var product2 = service.Create(new Product { SKU = "PROD-002", Price = 20m });
            var product3 = service.Create(new Product { SKU = "PROD-003", Price = 30m });

            // Act
            service.Delete(product2);

            // Assert
            var allProducts = service.GetAll().ToList();
            Assert.Equal(2, allProducts.Count);
            Assert.Contains(allProducts, p => p.Id == product1.Id);
            Assert.Contains(allProducts, p => p.Id == product3.Id);
            Assert.DoesNotContain(allProducts, p => p.Id == product2.Id);
        }

        #endregion
    }
}
