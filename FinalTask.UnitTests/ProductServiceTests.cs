using FinalTask.Application.Dtos;
using FinalTask.Application.Exceptions;
using FinalTask.Application.Services;
using FinalTask.Domain.Models;
using FinalTask.Infrastucture.Contracts;
using Moq;

namespace FinalTask.UnitTests
{
    public class ProductServiceTests
    {
        private List<Product> GetFakeProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name= "Test",
                    Description= "Test",
                    Price = 1111
                },
                new Product
                {
                    Id = 2,
                    Name= "Test2",
                    Description= "Test2",
                    Price = 2222
                }
            };
        }

        [Fact]
        public async Task GetProductListAsync_ShouldReturnProductList_WithData()
        {
            // Arrange
            var productList = GetFakeProducts();

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(r => r.GetAllAsync())
                    .Returns(Task.FromResult(productList));
            var productService = new ProductService(productRepository.Object);

            // Act
            var result = await productService.GetProductListAsync();

            // Assert
            Assert.IsType<List<Product>>(result);
            Assert.Equal(productList.Count, result.Count);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnProduct_WithValidId()
        {
            // Arrange
            int id = 1;

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(r => r.GetByIdAsync(id))
                    .Returns(Task.FromResult(new Product() {
                        Id = id,
                        Name = "Test",
                        Description = "Test",
                        Price = 123
                    }));

            var productService = new ProductService(productRepository.Object);

            // Act
            var result = await productService.GetProductByIdAsync(id);

            // Assert
            Assert.IsType<ProductModel>(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal("Test", result.Description);
            Assert.Equal(123, result.Price);
        }

        [Fact]
        public void GetProductAsync_ShouldThrowModelNotFoundException_WithInvalidId()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<Product>(null));
            var problemService = new ProductService(productRepository.Object);

            // Act
            Func<Task> response = async () => await problemService.GetProductByIdAsync(6);

            // Act & Assert
            Assert.ThrowsAsync<ModelNotFoundException>(response);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldReturnWithoutException_WithValidId()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(c => c.CreateAsync(It.IsAny<Product>()));
            var productService = new ProductService(productRepository.Object);

            // Act
            var result = Record.ExceptionAsync(async () => await productService.CreateProductAsync(new ProductModel()));

            // Assert
            Assert.Null(result.Result);
        }

        [Fact]
        public void UpdateProductAsync_ShouldUpdateProduct_WithValidId()
        {
            // Arrange
            var id = 1;


            var product = new Product()
            {
                Id = id,
                Name = "Test",
                Description= "Test",
                Price = 1
            };

            var productModel = new ProductModel()
            {
                Name = "Test2",
                Description = "Test2",
                Price = 2
            };

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(r => r.GetByIdAsync(id))
                    .Returns(Task.FromResult(product));
            productRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>()));

            var productService = new ProductService(productRepository.Object);

            // Act
            var result = productService.UpdateProductAsync(id, productModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(productModel.Name, product.Name);
                Assert.Equal(productModel.Description, product.Description);
                Assert.Equal(productModel.Price, product.Price);
            });
        }

        [Fact]
        public void UpdateProductAsync_ShouldReturnModelNotFoundException_WithInvalidProductId()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<Product>(null));
            productRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .Verifiable();

            var productService = new ProductService(productRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<ModelNotFoundException>(async () =>
                await productService.UpdateProductAsync(3, new ProductModel()));
            Assert.Throws<MockException>(() => productRepository.Verify());
        }

        [Fact]
        public void DeleteProductAsync_ShouldReturnWithoutException_WithValidId()
        {
            // Arrange
            int id = 1;
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(r => r.GetByIdAsync(id))
                    .Returns(Task.FromResult(new Product() { Id = id }));
            productRepository.Setup(r => r.DeleteAsync(It.IsAny<Product>()));
            var productService = new ProductService(productRepository.Object);

            // Act
            var result = productService.DeleteProductAsync(id);

            // Act & Assert
            Assert.Null(result.Exception);
        }

        [Fact]
        public void DeleteProductAsync_ShouldReturnModelNotFoundException_WithInvalidId()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<Product>(null));
            productRepository.Setup(r => r.DeleteAsync(It.IsAny<Product>()));

            var productService = new ProductService(productRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<ModelNotFoundException>(async () =>
                await productService.DeleteProductAsync(7));
        }
    }
}
