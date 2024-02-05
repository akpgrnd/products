using Microsoft.Net.Http.Headers;
using System.Net;

namespace Products.Api.IntegrationTests.Products;

public class Products_ByColour_Authz_Tests
{
    private static string Url(string colour) => $"/api/v1/products/{colour}";

    [Fact]
    public async Task Products_ByColourEndpoint_ReturnsUnauthorized_If_ApiKeyNotProvided()
    {
        var sut = CustomWebAppFactory.GetFactory().CreateClient();

        var response = await sut.GetAsync(Url("black"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("dlhd")]
    [InlineData("a2ce40c3-c1de-4158-8e0f-37cf1b557d0b")] // guid which is not in repo
    public async Task Products_ByColourEndpoint_ReturnsUnauthorized_If_ApiKeyIsInvalid(string apikey)
    {
        var sut = CustomWebAppFactory.GetFactory().CreateClient();
        
        sut.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, apikey);

        var response = await sut.GetAsync(Url("black"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }    
}
