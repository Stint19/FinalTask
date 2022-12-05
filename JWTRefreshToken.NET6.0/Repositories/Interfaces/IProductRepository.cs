using FinalTask.NET6._0.Models.ProductController;

namespace FinalTask.NET6._0.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> Get();
        Product GetById(int id);
        Product Post(Product product);
        Product Put(Product product);
        bool Delete(int id);

    }
}
