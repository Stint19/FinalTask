using AutoMapper;
using FinalTask.Application.Dtos;
using FinalTask.Application.Exceptions;
using FinalTask.Application.Services.Contracts;
using FinalTask.Domain.Models;
using FinalTask.Infrastucture.Contracts;

namespace FinalTask.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;    
            _productRepository= productRepository;
        }

        public async Task<int> CreateProductAsync(ProductModel productModel)
        {
            var product = await _productRepository.GetByNameAsync(productModel.Name);
            if (product != null)
            {
                throw new EntityAlreadyExistException(product.Name, product.Id);
            }

            var item = _mapper.Map<Product>(productModel);
            await _productRepository.CreateAsync(item);
            await _productRepository.SaveAsync();

            return item.Id;
        }

        public async Task DeleteProductAsync(int id)
        {
            var item = await _productRepository.GetByIdAsync(id);
            if (item is null)
            {
                throw new EntityNotFoundException("Product", id);
            }
            await _productRepository.DeleteAsync(item);
            await _productRepository.SaveAsync();
        }

        public async Task<ProductModel> GetProductByIdAsync(int productId)
        {
            var item = await _productRepository.GetByIdAsync(productId);
            if (item == null)
            {
                throw new EntityNotFoundException("Product", productId);
            }
            return _mapper.Map<ProductModel>(item);
        }

        public async Task<List<Product>> GetProductListAsync()
        {
            var items = await _productRepository.GetAllAsync();
            return items;
        }

        public async Task UpdateProductAsync(int id, ProductModel product)
        {
            var item = await _productRepository.GetByNameAsync(product.Name);
            if (item != null)
            {
                throw new EntityAlreadyExistException(item.Name, item.Id);
            }

            var existingItem = await _productRepository.GetByIdAsync(id);
            if (existingItem is null)
            {
                throw new EntityNotFoundException("Product", id);
            }

            _mapper.Map(product, existingItem);

            await _productRepository.UpdateAsync(existingItem);
            await _productRepository.SaveAsync();
        }
    }
}
