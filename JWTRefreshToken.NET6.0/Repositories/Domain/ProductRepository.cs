using FinalTask.NET6._0.Auth;
using FinalTask.NET6._0.Models.ProductController;
using FinalTask.NET6._0.Repositories.Interfaces;

namespace JWTRefreshToken.NET6._0.Repositories.Domain
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Delete(int id)
        {
            var item = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            _context.Products.Remove(item);
            _context.SaveChanges();

            return true;
        }

        public List<Product> Get()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            var item = _context.Products.FirstOrDefault(x => x.Id == id);
            return item;
        }

        public Product Post(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public Product Put(Product product)
        {
            var item = _context.Products.Where(x => x.Id == product.Id).FirstOrDefault();

            item.Name = product.Name;
            item.Price = product.Price;

            _context.SaveChanges();

            return item;
        }

    }
}
