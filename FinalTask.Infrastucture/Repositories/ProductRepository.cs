using FinalTask.Domain.Models;
using FinalTask.Infrastucture.Contracts;
using FinalTask.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;

namespace FinalTask.Infrastucture.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateAsync(Product item)
        {
            await _appDbContext.Products.AddAsync(item);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product item)
        {
            _appDbContext.Products.Update(item);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product item)
        {
            _appDbContext.Products.Remove(item);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var items = await _appDbContext.Products.ToListAsync();
            return items;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var item = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _appDbContext.Products.SingleOrDefaultAsync(x => x.Name == name);
        }
    }
}
