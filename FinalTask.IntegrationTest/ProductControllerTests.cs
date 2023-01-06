using Microsoft.AspNetCore.Hosting;
using FinalTask.WebApi;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text;
using FinalTask.Application.Dtos;
using FluentAssertions;
using FinalTask.Domain.Models;
using FinalTask.Application.Services.Contracts;
using FinalTask.Application.Services;
using FluentAssertions.Execution;

namespace FinalTask.IntegrationTest
{
    public class ProductControllerTests : IClassFixture<DefaultWebAppFactory<Program>>
    {
        private readonly DefaultWebAppFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly string _token;

        public ProductControllerTests(DefaultWebAppFactory<Program> factory)
        {

            _factory= factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var user = new
            {
                userName = "Admin",
                password = "String123@"
            };
            var userJson = JsonConvert.SerializeObject(user);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = _client.PostAsync("api/Authenticate/login", data).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            _token = JsonConvert.DeserializeObject<TokenModel>(responseString).AccessToken;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [Fact]
        public async Task GetProductList_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("api/Product");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseProductList = JsonConvert.DeserializeObject<List<Product>>(responseString);

            responseProductList.Should().NotBeNullOrEmpty();
            
        }

        [Fact]
        public async Task GetProduct_WithValidId_ShouldReturnOk()
        {
            // Arrange

            var productModel = new ProductModel
            {
                Name = "Test55456456",
                Description = "Description",
                Price = 777
            };
            var productJson = JsonConvert.SerializeObject(productModel);
            var data = new StringContent(productJson, Encoding.UTF8, "application/json");
            var productPost = await _client.PostAsync("api/Product", data);
            var responseString = await productPost.Content.ReadAsStringAsync();
            var productId = JsonConvert.DeserializeObject<int>(responseString);

            // Act
            var response = await _client.GetAsync("api/Product/" + productId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNull(responseString);
            var result = JsonConvert.DeserializeObject<ProductModel>(responseString);
            result.Should().BeEquivalentTo(productModel);
        }

        [Fact]
        public async Task GetProduct_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var productId = 100500;

            // Act
            var response = await _client.GetAsync("api/Product/" + productId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProduct_WithValidObject_ShouldReturnOk()
        {
            // Arrange

            var product = new ProductModel()
            {
                Name = "CreateTest",
                Description = "CreateTest",
                Price = 123123
            };
            var productJson = JsonConvert.SerializeObject(product);
            var data = new StringContent(productJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Product", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseString = await response.Content.ReadAsStringAsync();

            var productId = JsonConvert.DeserializeObject<int>(responseString);
            response = await _client.GetAsync("api/Product/" + productId);
            responseString = await response.Content.ReadAsStringAsync();
            var productResponse = JsonConvert.DeserializeObject<ProductModel>(responseString);

            productResponse.Should().NotBeNull();
            productResponse.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task CreateProduct_WithInvalidObject_ShouldReturnBadRequest()
        {
            // Arrange
            var problem = new ProductModel();
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Product", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var response = await _client.GetAsync("api/Product");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var productId = JsonConvert.DeserializeObject<List<Product>>(responseString)[0].Id;

            // Act
            response = await _client.DeleteAsync("api/Product/" + productId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            response = await _client.GetAsync("api/Product/" + productId);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidId_ShouldReturnModelNotFound()
        {
            // Arrange
            var problemId = 777;

            // Act
            var response = await _client.DeleteAsync("api/Product/" + problemId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateProduct_WithValidObject_ShouldReturnNoContent()
        {
            // Arrange

            var response = await _client.GetAsync("api/Product");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var productId = JsonConvert.DeserializeObject<List<Product>>(responseString)[0].Id;

            var productModel = new ProductModel()
            {
                Name = "UpdateString",
                Description = "Description",
                Price = 767676
            };

            var productJson = JsonConvert.SerializeObject(productModel);
            var data = new StringContent(productJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PutAsync("api/Product/" + productId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            response = await _client.GetAsync("api/Product/" + productId);
            responseString = await response.Content.ReadAsStringAsync();
            var productResponse = JsonConvert.DeserializeObject<ProductModel>(responseString);

            productResponse.Should().NotBeNull();
            productResponse.Should().BeEquivalentTo(productModel);

        }

        [Fact]
        public async Task UpdateProduct_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var productId = 11111111;

            var productModel = new ProductModel()
            {
                Name = "updateFailString",
                Description = "Description",
                Price = 123123123
            };

            var problemForUpdateJson = JsonConvert.SerializeObject(productModel);
            var data = new StringContent(problemForUpdateJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("api/Product/" + productId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Product");
        }
    }
}
