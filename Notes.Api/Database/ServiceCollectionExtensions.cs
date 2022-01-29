namespace Notes.Api.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotesDatabase(this IServiceCollection services) =>
        services.AddDbContext<NotesDb>(options => options.UseSqlite("Data Source=notes.db"));
}