using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Notes.Api.AccessControl;
using Notes.Api.Configuration;
using Notes.Api.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureSecrets(builder.Configuration)
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services
    .AddAccessControl()
    .AddSwaggerDocumentation()
    .AddNotesDatabase();

var application = builder.Build();

application
    .UseDeveloperExceptionPage()
    .UseNotesClientServer(application.Environment)
    .UseSwaggerDocumentation()
    .UseRouting()
    .UseAccessControl()
    .UseCorrelationId()
    .UseEndpoints(endpoints => endpoints.MapControllers());

application
    .SeedNotesDatabase()
    .Run();
