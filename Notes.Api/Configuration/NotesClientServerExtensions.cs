namespace Notes.Api.Configuration;

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

public static class NotesClientServerExtensions
{
    public static IApplicationBuilder UseNotesClientServer(this IApplicationBuilder application, IWebHostEnvironment environment) =>
        application.UseFileServer(new FileServerOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(environment.ContentRootPath, "../Notes.Client")),
            RequestPath = "/client",
            EnableDirectoryBrowsing = true
        });
}