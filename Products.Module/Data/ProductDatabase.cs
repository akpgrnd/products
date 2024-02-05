using Products.Module.Models;

namespace Products.Module.Data;
internal abstract class ProductDatabase
{
    internal abstract Task<IEnumerable<Product>> All { get; }
}
