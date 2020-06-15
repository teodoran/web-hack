using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Api.Models;
using Xunit;

namespace Notes.Api.Test
{
    public class PostNote : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly NotesApiClient _client;

        public PostNote(WebApplicationFactory<Startup> applicationFactory)
        {
            var httpClient = applicationFactory.CreateClient();
            _client = new NotesApiClient(httpClient);
        }

        [Fact]
        public async Task ShouldCreateANote()
        {
            var author = "agnes-rodgers";
            var content = "Bring drinks to the party next sunday.";
            (var createdNote, var statusCode) = await _client.PostNote(new CreateNote
            {
                Author = author,
                Content = content
            });

            createdNote.Author.Should().Be(author);
            createdNote.Content.Should().Be(content);
            statusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("pablo-diego-josé-francisco-de-paula-juan-nepomuceno-maría-de-los-remedios-cipriano-de-la-santísima-trinidad-ruiz-y-picasso")]
        [InlineData("")]
        [InlineData(null)]
        public async Task ShouldNotAcceptBadAuthors(string badAuthor)
        {
            (_, var statusCode) = await _client.PostNote(new CreateNote
            {
                Author = badAuthor,
                Content = "The old man was dreaming about the lions."
            });

            statusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Bella Swan moves from Phoenix, Arizona to live with her father in Forks, Washington to allow her mother to travel with her new husband, a minor league baseball player. After moving to Forks, Bella finds herself involuntarily drawn to a mysterious, handsome boy, Edward Cullen and eventually learns that he is a member of a vampire family which drinks animal blood rather than human blood. Edward and Bella fall in love, while James, a sadistic vampire from another coven, is drawn to hunt down Bella. Edward and the other Cullens defend Bella. She escapes to Phoenix, where she is tricked into confronting James, who tries to kill her. She is seriously wounded, but Edward rescues her and they return to Forks.")]
        public async Task ShouldNotAcceptBadContent(string badContent)
        {
            (_, var statusCode) = await _client.PostNote(new CreateNote
            {
                Author = "leo-tolstoy",
                Content = badContent
            });

            statusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public void Dispose() => _client.Dispose();
    }
}
