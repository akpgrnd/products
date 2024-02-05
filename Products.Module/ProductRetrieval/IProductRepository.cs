using Products.Module.Models;

namespace Products.Module;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<IEnumerable<Product>> GetProductsByColour(string colour);
}
