using FinalTask.Application.Dtos;
using FinalTask.Domain.Models;
using FinalTask.Domain.Models.Identity;
using FinalTask.Infrastucture.Data;
using Microsoft.AspNetCore.Identity;

namespace FinalTask.IntegrationTest
{
    public static class DbOption
    {
        public static async void Initialize(AppDbContext db,
                                      UserManager<AppUser> userManager,
                                      RoleManager<IdentityRole> roleManager)
        {
            
            AppUser user = new()
            {
                Email = "asd@asd.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Admin"
            };

            await userManager.CreateAsync(user, "String123@");

            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            await userManager.AddToRoleAsync(user, UserRoles.Admin);
            await userManager.AddToRoleAsync(user, UserRoles.User);

            var products = GetSeedProduct();
            db.Products.AddRange(products);
            db.SaveChanges();
            
        }

        public static async void ReInitialize(AppDbContext db)
        {
            var products = GetSeedProduct();
            db.Products.RemoveRange(products);
            db.Products.AddRange(products);        
        }

        public static Product[] GetSeedProduct()
        {
            return new Product[]
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

        public static Product[] GetSeedUsers()
        {
            return new Product[]
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
    }
}
