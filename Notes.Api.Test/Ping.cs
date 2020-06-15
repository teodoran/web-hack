using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Notes.Api.Test
{
    public class Ping : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly NotesApiClient _client;

        public Ping(WebApplicationFactory<Startup> applicationFactory)
        {
            var httpClient = applicationFactory.CreateClient();
            _client = new NotesApiClient(httpClient);
        }

        [Fact]
        public async Task TestGrats()
        {
            var response = await _client.Ping();
            response.Should().Be("PONG");
        }

        public void Dispose() => _client.Dispose();
    }
}
