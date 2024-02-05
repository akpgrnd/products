using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Products.Module;
using Products.Module.Models;
using System.Net.Http.Headers;

namespace Products.Api.IntegrationTests
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        public static Guid ValidApiKey = Guid.Parse("e5a1bc27-9d1d-41aa-9912-99eb11efa2aa");

        public static WebApplicationFactory<Program> GetFactory(Action<IServiceCollection>? configureServices = null)
        {
            var userRepoMock = Substitute.For<IUserRepository>();
            userRepoMock.GetUserByApiKey(ValidApiKey).Returns(new User("LoggedInUser"));

            var factory =
                new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.Replace(ServiceDescriptor.Singleton(userRepoMock));
                        configureServices?.Invoke(services);
                    });
                });

            return factory;
        }

        public static HttpClient GetAuthorizedClient(Action<IServiceCollection>? configureServices = null)
        {
            var client = GetFactory(configureServices).CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ValidApiKey.ToString());

            return client;
        }
    }
}
