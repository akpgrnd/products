using Microsoft.Net.Http.Headers;
using System.Net;

namespace Products.Api.IntegrationTests.Products;

public class Products_GetAll_Authz_Tests
{
    private const string Url = "/api/v1/products";

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
        
        sut.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, apikey);

        var response = await sut.GetAsync(Url);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
