using AutoMapper;
using FinalTask.Application.Dtos;
using FinalTask.Application.Exceptions;
using FinalTask.Application.Mapper;
using FinalTask.Application.Services;
using FinalTask.Domain.Models;
using FinalTask.Infrastucture.Contracts;
using FluentAssertions;
using Moq;

namespace FinalTask.UnitTests
{
    public class ProductServiceTests
    {
        private IMapper _mapper;
        private readonly Mock<IProductRepository> _productRepository;

        public ProductServiceTests()
        {
            _productRepository = new Mock<IProductRepository>();
            var mappingConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapProfiles());
            });
            _mapper = mappingConfiguration.CreateMapper();

        }

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
        public async Task GetProductListAsync_WithData_ShouldReturnProductList()
        {
            // Arrange
            var productList = GetFakeProducts();
            _productRepository.Setup(r => r.GetAllAsync())
                    .Returns(Task.FromResult(productList));
            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act
            var result = await productService.GetProductListAsync();

            // Assert
            Assert.IsType<List<Product>>(result);
            Assert.Equal(productList.Count, result.Count);
            Assert.Equal(result, productList);
        }

        [Fact]
        public async Task GetProductAsync_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            int id = 1;
            _productRepository.Setup(r => r.GetByIdAsync(id))
                    .Returns(Task.FromResult(new Product() {
                        Id = id,
                        Name = "Test",
                        Description = "Test",
                        Price = 123
                    }));

            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act
            var result = await productService.GetProductByIdAsync(id);

            // Assert
            Assert.IsType<ProductModel>(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal("Test", result.Description);
            Assert.Equal(123, result.Price);
        }

        [Fact]
        public void GetProductAsync_WithInvalidId_ShouldThrowModelNotFoundException()
        {
            // Arrange
            _productRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<Product>(null));
            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act
            Func<Task> response = async () => await productService.GetProductByIdAsync(6);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(response);
        }



        [Fact]
        public async Task CreateProductAsync_WithValidData_ShouldReturnId()
        {
            // Arrange
            var productModel = new ProductModel
            {
                Name = "Tes2323t",
                Description = "Te23st",
                Price = 1111
            };
            _productRepository.Setup(r => r.GetByNameAsync(productModel.Name));
            _productRepository.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(1);
            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act
            var result = await productService.CreateProductAsync(productModel);

            // Assert
            Assert.Equal(result, 1);
        }

        [Fact]
        public async Task CreateProductAsync_WithAlreadyExistEntity_ShouldReturnArgumentException()
        {
            // Arrange
            var productModel = new ProductModel
            {
                Name = "Tes2323t",
                Description = "Te23st",
                Price = 1111
            };
            _productRepository.Setup(r => r.GetByNameAsync(productModel.Name))
                .ReturnsAsync(new Product());
            _productRepository.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(1);
            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act
            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await productService.CreateProductAsync(productModel));
        }

        [Fact]
        public void UpdateProductAsync_WithValidId_ShouldUpdateProduct()
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

            _productRepository.Setup(r => r.GetByIdAsync(id))
                    .Returns(Task.FromResult(product));
            _productRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>()));

            var productService = new ProductService(_productRepository.Object, _mapper);

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
        public void UpdateProductAsync_WithInvalidProductId_ShouldReturnEntityNotFoundException()
        {
            // Arrange
            _productRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<Product>(null));
            _productRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .Verifiable();

            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.UpdateProductAsync(3, new ProductModel()));
            _productRepository.Verify(service => service.UpdateAsync(It.IsAny<Product>()), Times.Never);
            
        }

        [Fact]
        public void DeleteProductAsync_WithValidId_ShouldReturnWithoutException()
        {
            // Arrange
            int id = 1;
            _productRepository.Setup(r => r.GetByIdAsync(id))
                    .Returns(Task.FromResult(new Product() { Id = id }));
            _productRepository.Setup(r => r.DeleteAsync(It.IsAny<Product>()));
            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act
            var result = productService.DeleteProductAsync(id);

            // Act & Assert
            Assert.Null(result.Exception);
        }

        [Fact]
        public void DeleteProductAsync_WithInvalidId_ShouldReturnModelNotFoundException()
        {
            // Arrange
            _productRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<Product>(null));
            _productRepository.Setup(r => r.DeleteAsync(It.IsAny<Product>()));

            var productService = new ProductService(_productRepository.Object, _mapper);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.DeleteProductAsync(7));
        }
    }
}
