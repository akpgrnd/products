using Products.Module.Data;
using Products.Module.Models;

namespace Products.Module.MockData;

internal class ProductDatabaseStub : ProductDatabase
{
    internal override Task<IEnumerable<Product>> All => Task.FromResult(new Product[] 
    {
        new(1, "Samsung Refrigirator", "Silver", "Kitchen Appliances"),
        new(1, "Samsung Refrigirator - large", "White", "Kitchen Appliances"),
        new(13, "Panasonic Microwave", "White", "Kitchen Appliances"),
        new(18, "Induction Hob", "Black", "Kitchen Appliances"),
    }.AsEnumerable());
}
