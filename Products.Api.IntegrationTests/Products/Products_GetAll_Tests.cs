using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Products.Module;
using Products.Module.Models;
using System.Net;
using System.Net.Http.Json;

namespace Products.Api.IntegrationTests.Products;

public class Products_GetAll_Tests
{
    private const string Url = "/api/v1/products";

    [Fact]
    public async Task ProductsEndpoint_Returns_OK()
    {
        var sut = CustomWebAppFactory.GetAuthorizedClient();

        var response = await sut.GetAsync(Url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Products_GetAllEndpoint_ReturnsListOfProducts()
    {
        List<Product> productData = [
            new(12, "Office chair", "black", "Office furniture"),
            new(13, """27" HP Monitor""", "black", "Office electronic equipment"),
            new(18, "Notebook - A4", "red", "Office supplies"),
        ];

        var productRepoMock = Substitute.For<IProductRepository>();
        productRepoMock.GetAll().Returns(productData);

        var sut = CustomWebAppFactory.GetAuthorizedClient(services 
            => services.Replace(ServiceDescriptor.Singleton(productRepoMock)));

        var result = await sut.GetFromJsonAsync<Product[]>(Url);

        Assert.Equal(productData, result);
    }

    [Fact]
    public async Task Products_GetAllEndpoint_ReturnsUnauthorized_If_ApiKeyNotProvided()
    {
        var sut = CustomWebAppFactory.GetFactory().CreateClient();

        var response = await sut.GetAsync(Url);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("dlhd")]
    [InlineData("a2ce40c3-c1de-4158-8e0f-37cf1b557d0b")] // guid which is not in repo
    public async Task Products_GetAllEndpoint_ReturnsUnauthorized_If_ApiKeyIsInvalid(string apikey)
    {
        var sut = CustomWebAppFactory.GetFactory().CreateClient();
        
        sut.DefaultRequestHeaders.TryAddWithoutValidation(HttpRequestHeader.Authorization.ToString(), apikey);

        var response = await sut.GetAsync(Url);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
