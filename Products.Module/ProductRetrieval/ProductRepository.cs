using Products.Module.Data;
using Products.Module.Models;

namespace Products.Module;

// this class returns mock data only
internal class ProductRepository(ProductDatabase data) : IProductRepository
{
    public Task<IEnumerable<Product>> GetAll() => data.All;

    public async Task<IEnumerable<Product>> GetProductsByColour(string colour)
    {
        var result = (await data.All)
            .Where(x => x.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return result;
    }
} 
