using FinalTask.Application.Dtos;
using FinalTask.Application.Exceptions;
using FinalTask.Application.Services.Contracts;
using FinalTask.Domain.Models;
using FinalTask.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinalTask.UnitTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productService;

        public ProductControllerTests()
        {
            _productService = new Mock<IProductService>();
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
        public async Task GetProductList_WithData_ShouldReturnOk()
        {
            //Arrange
            _productService.Setup(c => c.GetProductListAsync())
                .ReturnsAsync(GetFakeProducts());

            var controller = new ProductController(_productService.Object);

            //Act
            var response = await controller.Get();

            //Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.IsType<List<Product>>(result.Value);
            var productList = result.Value as List<Product>;

            productList.Should().BeEquivalentTo(GetFakeProducts());
            
        }

        [Fact]
        public async Task GetProduct_WithValidId_ShouldReturnOk()
        {
            //Arrange
            var id = 1;
            var product = GetFakeProducts().Single(x => x.Id == id);
            _productService.Setup(c => c.GetProductByIdAsync(id))
                .Returns(Task.FromResult(new ProductModel
                {
                    Name= product.Name,
                    Description= product.Description,
                    Price= product.Price,
                }));

            var controller = new ProductController(_productService.Object);

            //Act
            var response = await controller.Get(id);

            //Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.IsType<ProductModel>(result.Value);
            var productFact = result.Value as ProductModel;

            Assert.Equal(productFact.Name,product.Name);
            Assert.Equal(productFact.Description, product.Description);
            Assert.Equal(productFact.Price, product.Price);
        }

        [Fact]
        public async Task GetProduct_WithNotValidId_ShouldReturnModelNotFoundException()
        {
            //Arrange
            var id = 4;
            _productService.Setup(c => c.GetProductByIdAsync(id))
                .Throws<EntityNotFoundException>();

            var controller = new ProductController(_productService.Object);

            //Act
            Func<Task> response = async () => await controller.Get(id);

            //Assert
            await response.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CreateProduct_WithValidObject_ShouldReturnOk()
        {
            // Arrange
            _productService.Setup(r => r.CreateProductAsync(It.IsAny<ProductModel>()));
            var request = new ProductModel();
            var controller = new ProductController(_productService.Object);

            // Act
            var response = await controller.CreateProduct(request);

            // Assert
            Assert.IsType<CreatedResult>(response);
        }

        [Fact]
        public async Task CreateProduct_WithInvalidObject_ShouldReturnArgumentException()
        {
            // Arrange
            var controller = new ProductController(null);
            controller.ModelState.AddModelError("Fields", "Required");
            var product = new ProductModel();
            
            //Act
            Func<Task> response = async () => await controller.CreateProduct(product);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(response);
        }

        [Fact]
        public async Task UpdateProduct_WithValidObject_ShouldReturnNoContent()
        {
            // Arrange
            var product = new ProductModel
            {
                Name = "Test123",
                Description = "Test",
                Price = 1111
            };
            _productService.Setup(c => c.UpdateProductAsync(1, product));
            var controller = new ProductController(_productService.Object);

            // Act
            var response = await controller.UpdateProduct(1, product);

            // Assert
            Assert.IsType<NoContentResult>(response);
            _productService.Verify(r =>
                r.UpdateProductAsync(1, product));
        }

        [Fact]
        public async Task UpdateProduct_WithInvalidObject_ShouldThrowArgumentException()
        {
            // Arrange
            var controller = new ProductController(null);
            controller.ModelState.AddModelError("Fields", "Required");

            //Act
            Func<Task> response = async () => await controller.UpdateProduct(1, new ProductModel());

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(response);
        }

        [Fact]
        public async Task UpdateProduct_WithInvalidId_ShouldThrowModelNotFoundException()
        {
            // Arrange
            _productService.Setup(r => r.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductModel>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProductController(_productService.Object);

            // Act
            Func<Task> act = async () => await controller.UpdateProduct(5, new ProductModel());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _productService.Setup(r => r.DeleteProductAsync(It.IsAny<int>()));
            var controller = new ProductController(_productService.Object);

            // Act
            var response = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(response);
            _productService.Verify(r =>
                r.DeleteProductAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task DeletePrduct_WithInvalidId_ShouldReturnModelNotFoundException()
        {
            // Arrange
            _productService.Setup(r => r.DeleteProductAsync(It.IsAny<int>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProductController(_productService.Object);

            // Act
            Func<Task> act = async () => await controller.Delete(6);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }
    }
}
