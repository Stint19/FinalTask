using FinalTask.Application.Dtos;
using FinalTask.Application.Exceprions;
using FinalTask.Application.Services.Contracts;
using FinalTask.Domain.Models;
using FinalTask.Infrastucture.Contracts;

namespace FinalTask.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository= productRepository;
        }

        public async Task CreateProductAsync(ProductModel product)
        {
            await CheckIfExistsAsync(product);
            var item = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };

            await _productRepository.CreateAsync(item);
        }

        public async Task DeleteProductAsync(int id)
        {
            var item = await _productRepository.GetByIdAsync(id);
            CheckIfFound(id, item);
            await _productRepository.DeleteAsync(item);
        }

        public async Task<ProductModel> GetProductByIdAsync(int productId)
        {
            var item = await _productRepository.GetByIdAsync(productId);
            CheckIfFound(productId, item);
            return new ProductModel
            {
                Name = item.Name,
                Description = item.Description,
                Price = item.Price
            };
        }

        public async Task<List<Product>> GetProductListAsync()
        {
            var items = await _productRepository.GetAllAsync();
            return items;
        }

        public async Task UpdateProductAsync(int id, ProductModel product)
        {
            await CheckIfExistsAsync(product);
            var item = await _productRepository.GetByIdAsync(id);
            CheckIfFound(id, item);
            item.Name = product.Name;
            item.Description = product.Description;
            item.Price = product.Price;
            await _productRepository.UpdateAsync(item);
        }

        private async Task CheckIfFound(int id, Product product)
        {
            if (product is null)
            {
                throw new ModelNotFoundException("Product", id);
            }
        }

        private async Task CheckIfExistsAsync(ProductModel model)
        {
            var bicycle = await _productRepository.GetByNameAsync(model.Name);
            if (bicycle != null)
            {
                throw new ArgumentException("This product already exist.");
            }
        }
    }
}
