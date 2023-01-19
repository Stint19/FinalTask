using FinalTask.Domain.Models.Identity;
using FinalTask.Infrastucture.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinalTask.IntegrationTest
{
    public class DefaultWebAppFactory<T>
        : WebApplicationFactory<T> where T : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(c =>
                    c.ServiceType == typeof(DbContextOptions<AppDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDb");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();
                    var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();
                    var userRoles = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    try
                    {
                        DbOption.Initialize(db, userManager, userRoles);
                    }
                    catch
                    {

                    }
                }
            });
        }
    }
}
