using FinalTask.Application.Services;
using FinalTask.Application.Services.Contracts;
using FinalTask.Infrastucture.Contracts;
using FinalTask.Infrastucture.Repositories;

namespace FinalTask.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();

        }
    }
}
