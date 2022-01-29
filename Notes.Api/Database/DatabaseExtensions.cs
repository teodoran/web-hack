namespace Notes.Api.Database;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DatabaseExtensions
{
    public static IServiceCollection AddNotesDatabase(this IServiceCollection services) =>
        services.AddDbContext<NotesDb>(options => options.UseSqlite("Data Source=notes.db"));

    public static WebApplication SeedNotesDatabase(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<NotesDb>();
        database.SeedData();

        return application;
    }
}