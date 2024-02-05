using System.Net;

namespace Products.Api.IntegrationTests.HealthChecks
{
    public class HealthChecks_Tests
    {
        private const string Url = "/";

        [Fact]
        public async Task Live_HealthCheckResponse_IsOk()
        {
            var factory = CustomWebAppFactory.GetFactory();

            var sut = factory.CreateClient();

            var response = await sut.GetAsync(Url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Live_HealthCheckResponse_Content_Has_StatusReady()
        {
            var factory = CustomWebAppFactory.GetFactory();

            var sut = factory.CreateClient();

            var response = await sut.GetAsync(Url);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal("Healthy", responseBody);
        }
    }
}
