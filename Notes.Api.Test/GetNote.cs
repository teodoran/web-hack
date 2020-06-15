using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Api.Models;
using Xunit;

namespace Notes.Api.Test
{
    public class GetNote : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly NotesApiClient _client;

        public GetNote(WebApplicationFactory<Startup> applicationFactory)
        {
            var httpClient = applicationFactory.CreateClient();
            _client = new NotesApiClient(httpClient);
        }

        [Fact]
        public async Task ShouldReturnASingleNote()
        {
            var author = "agnes-rodgers";
            var content = "Bring drinks to the party next sunday.";
            (var createdNote, _) = await _client.PostNote(new CreateNote
            {
                Author = author,
                Content = content
            });

            (var note, var statusCode) = await _client.GetNote(createdNote.Id);

            note.Author.Should().Be(author);
            note.Content.Should().Be(content);
            statusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ShouldReportWhenANoteIsNotFound()
        {
            (_, var statusCode) = await _client.GetNote(1704);

            statusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public void Dispose() => _client.Dispose();
    }
}
