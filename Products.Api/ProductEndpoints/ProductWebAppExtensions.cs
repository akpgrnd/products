using Microsoft.AspNetCore.Authentication;
using Products.Api.Auth;
using Products.Module;
using Products.Module.Models;
using System.Security.Claims;

namespace Products.Api.ProductEndpoints
{
    public static class ProductWebAppExtensions
    {
        public static WebApplication AddProductEndpoints(this WebApplication app)
        {
            app
                .MapGet("/products", (IProductRepository productsRepo) =>
                {
                    // depending on design it may worth mapping product object to a new response object, decoupling response from application model
                    // no point doing this here
                    return productsRepo.GetAll();
                })
                .RequireAuthorization("ProductUsers")
                .Produces<List<Product>>()
                .WithName("All products")
                .WithOpenApi();

            app
                .MapGet("/products/{colour}", (IProductRepository productsRepo, string colour) =>
                        productsRepo.GetProductsByColour(colour))
                .RequireAuthorization("ProductUsers")
                .Produces<List<Product>>()
                .AddEndpointFilter<ColourValidator>()
                .WithName("Products by colour")
                .WithOpenApi();

            return app;
        }

        public static IServiceCollection AddProductsAuthz(this IServiceCollection services)
        {
            services
                .AddAuthentication("ApiKey")
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>("ApiKey", _ => { });

            services
                .AddAuthorizationBuilder()
                .AddPolicy("ProductUsers", policy => policy.RequireClaim(ClaimTypes.Role, "RetrieveProducts"));

            return services;
        }
    }
}
