using FinalTask.Application.Dtos;
using FinalTask.Domain.Models;

namespace FinalTask.Application.Services.Contracts
{
    public interface IProductService
    {
        Task<List<Product>> GetProductListAsync();
        Task<ProductModel> GetProductByIdAsync(int productId);
        Task CreateProductAsync(ProductModel product);
        Task UpdateProductAsync(int id, ProductModel product);
        Task DeleteProductAsync(int id);
    }
}
