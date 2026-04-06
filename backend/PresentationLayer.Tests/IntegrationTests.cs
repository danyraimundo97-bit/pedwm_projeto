using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

namespace PresentationLayer.Tests.Integration
{
    // WebApplicationFactory sobe o Program.cs/Startup em memória
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GraphQL_BemVindo_ShouldReturnSuccess()
        {
            // Query GraphQL simples (bemVindo)
            var query = new { query = "{ bemVindo }" };
            var content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/graphql", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("API de Gestão de Projetos Online");
        }
    }
}