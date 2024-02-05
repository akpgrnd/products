using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Products.Module;
using Products.Module.Data;
using Products.Module.Models;
using System.Net;
using System.Net.Http.Json;

namespace Products.Api.IntegrationTests.Products;

public class Products_ByColour_Tests
{
    private static string Url(string colour) => $"/api/v1/products/{colour}";

    [Fact]
    public async Task Products_ByColourEndpoint_Returns_OK()
    {
        var sut = CustomWebAppFactory.GetAuthorizedClient();
        var response = await sut.GetAsync(Url("purple"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("ab")] // less than 3 characters
    [InlineData("veryLongColourXX")] // over 15 characters
    [InlineData("number5")] // numbers
    [InlineData("also spaces")] // spaces
    [InlineData("specchars!")] // special characters are not allowed
    public async Task Products_ByColourEndpoint_Returns_ValidationError_IfColourIsInvalid(string colour)
    {
        var sut = CustomWebAppFactory.GetAuthorizedClient();
        var response = await sut.GetAsync(Url(colour));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();

        Assert.Contains("One or more validation errors occurred.", body);
    }

    [Theory]
    [InlineData("black")]
    [InlineData("BLACK")]
    [InlineData("Black")]
    public async Task Products_ByColourEndpoint_ReturnsListOfProducts(string colour)
    {
        List<Product> productData = [
            new(12, "Office chair", "black", "Office furniture"),
            new(13, """27" HP Monitor""", "black", "Office electronic equipment"),
            new(18, "Notebook - A4", "red", "Office supplies"),
        ];

        var productRepo = new ProductRepository(new MockDb(productData));

        var sut = CustomWebAppFactory.GetAuthorizedClient(services 
            => services.Replace(ServiceDescriptor.Singleton<IProductRepository>(productRepo)));

        var result = await sut.GetFromJsonAsync<Product[]>(Url(colour));

        List<Product> expected = [
            new(12, "Office chair", "black", "Office furniture"),
            new(13, """27" HP Monitor""", "black", "Office electronic equipment")
        ];

        Assert.Equal(expected, result);
    }

    private class MockDb(IEnumerable<Product> productData) : ProductDatabase
    {
        internal override Task<IEnumerable<Product>> All => Task.FromResult(productData);
    }
    
}
