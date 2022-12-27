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
        public async Task GetProductList_ShouldReturnOk_WithData()
        {
            //Arrange
            var productService = new Mock<IProductService>();
            productService.Setup(c => c.GetProductListAsync())
                .Returns(Task.FromResult(GetFakeProducts()));

            var controller = new ProductController(productService.Object);

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
        public async Task GetProduct_ShouldReturnOk_WithValidId()
        {
            //Arrange
            var id = 1;
            var product = GetFakeProducts().Single(x => x.Id == id);
            var productService = new Mock<IProductService>();
            productService.Setup(c => c.GetProductByIdAsync(id))
                .Returns(Task.FromResult(new ProductModel
                {
                    Name= product.Name,
                    Description= product.Description,
                    Price= product.Price,
                }));

            var controller = new ProductController(productService.Object);

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
        public async Task GetProduct_ShouldReturnModelNotFoundException_WithNotValidId()
        {
            //Arrange
            var id = 4;
            var productService = new Mock<IProductService>();
            productService.Setup(c => c.GetProductByIdAsync(id))
                .Throws<EntityNotFoundException>();

            var controller = new ProductController(productService.Object);

            //Act
            Func<Task> response = async () => await controller.Get(id);

            //Assert
            await response.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnOk_WithValidObject()
        {
            // Arrange
            var productService = new Mock<IProductService>();
            productService.Setup(r => r.CreateProductAsync(It.IsAny<ProductModel>()));
            var request = new ProductModel();
            var controller = new ProductController(productService.Object);

            // Act
            var response = await controller.CreateProduct(request);

            // Assert
            Assert.IsType<CreatedResult>(response);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnArgumentException_WithInvalidObject()
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
        public async Task UpdateProduct_ShouldReturnNoContent_WithValidObject()
        {
            // Arrange
            var productService = new Mock<IProductService>();
            productService.Setup(c => c.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductModel>()));
            var controller = new ProductController(productService.Object);

            // Act
            var response = await controller.UpdateProduct(1, new ProductModel());

            // Assert
            Assert.IsType<NoContentResult>(response);
            productService.Verify(r =>
                r.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductModel>()));
        }

        [Fact]
        public async Task UpdateProduct_ShouldThrowArgumentException_WithInvalidObject()
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
        public async Task UpdateProduct_ShouldThrowModelNotFoundException_WithInvalidId()
        {
            // Arrange
            var productService = new Mock<IProductService>();
            productService.Setup(r => r.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductModel>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProductController(productService.Object);

            // Act
            Func<Task> act = async () => await controller.UpdateProduct(5, new ProductModel());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WithValidId()
        {
            // Arrange
            var productService = new Mock<IProductService>();
            productService.Setup(r => r.DeleteProductAsync(It.IsAny<int>()));
            var controller = new ProductController(productService.Object);

            // Act
            var response = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(response);
            productService.Verify(r =>
                r.DeleteProductAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task DeletePrduct_ShouldReturnModelNotFoundException_WithInvalidId()
        {
            // Arrange
            var productService = new Mock<IProductService>();
            productService.Setup(r => r.DeleteProductAsync(It.IsAny<int>()))
                .Throws<ModelNotFoundException>();

            var controller = new ProductController(productService.Object);

            // Act
            Func<Task> act = async () => await controller.Delete(6);

            // Assert
            await act.Should().ThrowAsync<ModelNotFoundException>();
        }
    }
}
