using AutoFixture;
using FinalTask.NET6._0.Controllers;
using FinalTask.NET6._0.Models.ProductController;
using FinalTask.NET6._0.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductRepository> _productRepository;
        private Fixture _fixture;
        private ProductController _controller;

        public ProductControllerTests()
        {
            _fixture = new Fixture();
            _productRepository = new Mock<IProductRepository>();
        }

        [TestMethod]
        public async Task Get_Product_ReturnOk()
        {
            var productList = _fixture.CreateMany<Product>(3).ToList();
            _productRepository.Setup(rep => rep.Get()).Returns(productList);
            _controller = new ProductController(_productRepository.Object);

            var result = await _controller.Get();
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_Product_ThrowExceprion()
        {
            _productRepository.Setup(rep => rep.Get()).Throws(new Exception());
            _controller = new ProductController(_productRepository.Object);

            var result = await _controller.Get();
            var obj = result as ObjectResult;

            Assert.AreEqual(400, obj.StatusCode);
        }

        [TestMethod]
        public async Task Post_Product_ReturnOk()
        {
            var product = _fixture.Create<Product>();

            _productRepository.Setup(repo => repo.Post(It.IsAny<Product>())).Returns(product);

            _controller = new ProductController(_productRepository.Object);

            var result = await _controller.Post(product);
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Put_Product_ReturnOk()
        {
            var product = _fixture.Create<Product>();

            _productRepository.Setup(repo => repo.Put(It.IsAny<Product>())).Returns(product);

            _controller = new ProductController(_productRepository.Object);

            var result = await _controller.Put(product);
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Product_ReturnOk()
        {
            _productRepository.Setup(repo => repo.Delete(It.IsAny<int>())).Returns(true);

            _controller = new ProductController(_productRepository.Object);

            var result = await _controller.Delete(It.IsAny<int>());
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj.StatusCode);
        }
    }
}
