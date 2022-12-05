using AutoFixture;
using FinalTask.NET6._0.Auth;
using FinalTask.NET6._0.Controllers;
using FinalTask.NET6._0.Models.ProductController;
using FinalTask.NET6._0.Repositories.Interfaces;
using JWTRefreshToken.NET6._0.Repositories.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask.UnitTests
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();

            if(await databaseContext.Products.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Products.Add(new Product
                    {
                        Id = i,
                        Name = "TestProduct",
                        Price = 1000
                    });
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        [TestMethod]
        public async Task ProductRepository_Add_ReturnProduct()
        {
            var product = new Product
            {
                Name = "TestProduct",
                Price = 1000
            };
            var dbContext = await GetDbContext();
            var productRepository = new ProductRepository(dbContext);

            var result = productRepository.Post(product);

            Assert.AreEqual(result, product);
        }

        [TestMethod]
        public async Task ProductRepository_GetById_ReturnNull()
        {
            var id = 1000;
            var dbContext = await GetDbContext();
            var productRepository = new ProductRepository(dbContext);

            var result = productRepository.GetById(id);

            Assert.IsNull(result);  
        }
    }
}
