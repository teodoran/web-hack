using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Notes.Api.Models;

namespace Notes.Api.Test
{
    public class NotesApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public NotesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Ping()
        {
            using (var response = await _httpClient.GetAsync("ping"))
            {
                return await response.AsString();
            }
        }

        public async Task<(Note[], HttpStatusCode)> GetNotes(string author)
        {
            using (var response = await _httpClient.GetAsync($"notes?author={author}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return (null, response.StatusCode);
                }

                var notes = await response.Deserialize<Note[]>();

                return (notes, response.StatusCode);
            }
        }

        public async Task<(Note, HttpStatusCode)> PostNote(CreateNote createNote)
        {
            var json = JsonConvert.SerializeObject(createNote);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("notes", content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return (null, response.StatusCode);
                }

                return (await response.Deserialize<Note>(), response.StatusCode);
            }
        }

        public async Task<(Note, HttpStatusCode)> GetNote(int noteId)
        {
            using (var response = await _httpClient.GetAsync($"notes/{noteId}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return (null, response.StatusCode);
                }

                return (await response.Deserialize<Note>(), response.StatusCode);
            }
        }

        public async Task<(Note, HttpStatusCode)> PatchNote(int noteId, UpdateNote updateNote)
        {
            var json = JsonConvert.SerializeObject(updateNote);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PatchAsync($"notes/{noteId}", content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return (null, response.StatusCode);
                }

                return (await response.Deserialize<Note>(), response.StatusCode);
            }
        }

        public void Dispose() => _httpClient.Dispose();
    }
}