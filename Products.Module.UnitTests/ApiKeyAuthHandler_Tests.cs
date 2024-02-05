using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using NSubstitute;
using Products.Api.Auth;
using Products.Module.Models;
using System.Text.Encodings.Web;

namespace Products.Module.UnitTests;
public class ApiKeyAuthHandler_Tests
{
    private readonly IOptionsMonitor<AuthenticationSchemeOptions> _options;
    private readonly ILoggerFactory _logger;
    private readonly UrlEncoder _encoder;
    private readonly IUserRepository _userService;

    public ApiKeyAuthHandler_Tests()
    {
        _options = Substitute.For<IOptionsMonitor<AuthenticationSchemeOptions>>();

        _options
            .Get(Arg.Any<string>())
            .Returns(new AuthenticationSchemeOptions());

        var logger = Substitute.For<ILogger<ApiKeyAuthHandler>>();
        _logger = Substitute.For<ILoggerFactory>();
        _logger
            .CreateLogger(Arg.Any<string>())
            .Returns(logger);

        _encoder = Substitute.For<UrlEncoder>();

        _userService = Substitute.For<IUserRepository>();
    }

    [Fact]
    public async Task Auth_Status_Failure_When_NoAuthorizationHeader()
    {
        var httpContext = new DefaultHttpContext();
        var sut = new ApiKeyAuthHandler(_userService, _options, _logger, _encoder);
        await sut.InitializeAsync(new AuthenticationScheme("ApiKey", null, typeof(ApiKeyAuthHandler)), httpContext);

        var result = await sut.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("Api Key not provided.", result.Failure!.Message);
    }

    [Fact]
    public async Task Auth_Status_Failure_When_ApiKey_IsNotAGuid()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[HeaderNames.Authorization] = "NOT A GUID";
        var sut = new ApiKeyAuthHandler(_userService, _options, _logger, _encoder);
        await sut.InitializeAsync(new AuthenticationScheme("ApiKey", null, typeof(ApiKeyAuthHandler)), httpContext);

        var result = await sut.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("Api Key invalid.", result.Failure!.Message);
    }

    [Fact]
    public async Task Auth_Status_Failure_When_ApiKey_IsNotInRepository()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[HeaderNames.Authorization] = "324fbede-63d9-4f22-81e8-2a61ba658d20";
        var sut = new ApiKeyAuthHandler(_userService, _options, _logger, _encoder);
        await sut.InitializeAsync(new AuthenticationScheme("ApiKey", null, typeof(ApiKeyAuthHandler)), httpContext);

        var result = await sut.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Empty(result.Failure!.Message);
    }

    [Fact]
    public async Task Auth_Status_Success_When_ApiKey_IsFoundInRepository()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[HeaderNames.Authorization] = "324fbede-63d9-4f22-81e8-2a61ba658d20";

        var userService = Substitute.For<IUserRepository>();
        userService.GetUserByApiKey(Guid.Parse("324fbede-63d9-4f22-81e8-2a61ba658d20"))
            .Returns(new User("VALID"));

        var sut = new ApiKeyAuthHandler(userService, _options, _logger, _encoder);
        await sut.InitializeAsync(new AuthenticationScheme("ApiKey", null, typeof(ApiKeyAuthHandler)), httpContext);

        var result = await sut.AuthenticateAsync();

        Assert.True(result.Succeeded);
    }
}
