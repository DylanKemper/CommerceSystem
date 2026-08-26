using Stockify.Models;

namespace Stockify.Services
{
    public interface IProductService
    {
        Product Add(Product product);
        IEnumerable<Product> GetAllProducts();
        Product? GetById(int id);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int id);
    }
}
