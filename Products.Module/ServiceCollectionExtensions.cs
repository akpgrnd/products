using Microsoft.Extensions.DependencyInjection;

namespace Products.Module;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProducts(this IServiceCollection services)
    {
        services
            .AddSingleton<IProductRepository>(new ProductRepository(new MockData.ProductDatabaseStub()))
            .AddSingleton<IUserRepository, UsersRepository>();

        return services;
    }
}
