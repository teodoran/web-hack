using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Api.Models;
using Xunit;

namespace Notes.Api.Test
{
    public class PatchNote : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly NotesApiClient _client;

        public PatchNote(WebApplicationFactory<Startup> applicationFactory)
        {
            var httpClient = applicationFactory.CreateClient();
            _client = new NotesApiClient(httpClient);
        }

        [Fact]
        public async Task ShouldReplaceANote()
        {
            var author = "annie-clark";
            (var createdNote, _) = await _client.PostNote(new CreateNote
            {
                Author = author,
                Content = "Remember to go jogging with Mike this afternoon."
            });

            var content = "Don't go fishing with Jen this morning.";
            (var replacedNote, var statusCode) = await _client.PatchNote(createdNote.Id, new UpdateNote
            {
                Content = content
            });

            replacedNote.Author.Should().Be(author);
            replacedNote.Content.Should().Be(content);
            replacedNote.Id.Should().Be(createdNote.Id);
            statusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ShouldReportWhenANoteIsNotFound()
        {
            (_, var statusCode) = await _client.PatchNote(1704, new UpdateNote
            {
                Content = "This could be anything really."
            });

            statusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Edward and his family leave Forks because he believes he is endangering Bella's life. Bella goes into a depression until she develops a strong friendship with Jacob Black, who she discovers can shape-shift into a wolf. Jacob and the other wolves in his tribe must protect her from Victoria, a vampire seeking to avenge the death of her mate James. Due to a misunderstanding, Edward believes Bella is dead. Edward decides to commit suicide in Volterra, Italy, but is stopped by Bella, who is accompanied by Edward's sister, Alice. They meet with the Volturi, a powerful vampire coven, and are released only on the condition that Bella be turned into a vampire in the near future. Bella and Edward are reunited, and she and the Cullens return to Forks.")]
        public async Task ShouldNotAcceptBadContent(string badContent)
        {
            (_, var statusCode) = await _client.PatchNote(0, new UpdateNote
            {
                Content = badContent
            });

            statusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public void Dispose() => _client.Dispose();
    }
}
