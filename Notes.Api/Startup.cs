namespace Notes.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Notes.Api.AccessControl;
using Notes.Api.Configuration;
using Notes.Api.Database;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .ConfigureSecrets(Configuration)
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddAccessControl();
        services.AddSwaggerDocumentation();
        services.AddNotesDatabase();
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment, NotesDb database)
    {
        database.SeedData();

        application
            .UseDeveloperExceptionPage()
            .UseNotesClientServer(environment)
            .UseSwaggerDocumentation()
            .UseRouting()
            .UseAccessControl()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}