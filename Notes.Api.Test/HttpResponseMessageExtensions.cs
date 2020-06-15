using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Notes.Api.Test
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<string> AsString(this HttpResponseMessage response)
        {
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(contentStream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        public static async Task<T> Deserialize<T>(this HttpResponseMessage response)
        {
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(contentStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();

                return serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}