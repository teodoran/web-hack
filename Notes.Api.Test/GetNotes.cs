using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Api.Models;
using Xunit;

namespace Notes.Api.Test
{
    public class GetNotes : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private const string Author = "david-lawrence";
        private readonly NotesApiClient _client;
        private readonly CreateNote[] _createNotes = new CreateNote[]
        {
            new CreateNote
            {
                Author = Author,
                Content = "Get milk and bread for breakfast."
            },
            new CreateNote
            {
                Author = Author,
                Content = "Remember to pick up the children at school on thursdays."
            }
        };

        public GetNotes(WebApplicationFactory<Startup> applicationFactory)
        {
            var httpClient = applicationFactory.CreateClient();
            _client = new NotesApiClient(httpClient);
        }

        [Fact]
        public async Task ShouldReturnAllNotes()
        {
            foreach (var createNote in _createNotes)
            {
                await _client.PostNote(createNote);
            }

            (var notes, var statusCode) = await _client.GetNotes(Author);

            statusCode.Should().Be(HttpStatusCode.OK);
            notes.Should().HaveCount(2)
                .And.OnlyContain(note => note.Author == Author)
                .And.ContainSingle(note => note.Content == _createNotes[0].Content)
                .And.ContainSingle(note => note.Content == _createNotes[1].Content);
        }

        [Fact]
        public async Task ShouldReturnNoNotes()
        {
            await _client.PostNote(_createNotes[0]);
            (var notes, var statusCode) = await _client.GetNotes("an-author-with-no-notes");

            notes.Should().HaveCount(0);
            statusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("pablo-diego-josé-francisco-de-paula-juan-nepomuceno-maría-de-los-remedios-cipriano-de-la-santísima-trinidad-ruiz-y-picasso")]
        [InlineData("")]
        [InlineData(null)]
        public async Task ShouldNotAcceptBadAuthors(string badAuthor)
        {
            (_, var statusCode) = await _client.GetNotes(badAuthor);

            statusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public void Dispose() => _client.Dispose();
    }
}
