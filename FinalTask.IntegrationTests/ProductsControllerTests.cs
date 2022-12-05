using FinalTask.NET6._0.Models.ProductController;
using FinalTask.NET6._0.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask.IntegrationTests
{
    [TestFixture]
    public class ProductsControllerTests
    {
        [Test]
        public async Task Get_ProductById_ShouldReturnUnauthorized()
        {
            int id = 1;
            WebApplicationFactory<Program> webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var repo = services.SingleOrDefault(d => d.ServiceType == typeof(IProductRepository));

                    services.Remove(repo);

                    var mockService = new Mock<IProductRepository>();

                    mockService.Setup(_ => _.GetById(id)).Returns(new Product { Id = 1, Name = "Test", Price = 1000});

                    services.AddTransient(_ => mockService.Object);
                });
            });
            HttpClient client = webHost.CreateClient();

            HttpResponseMessage response = await client.GetAsync("api/Product/1");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);

        }
    }
}
