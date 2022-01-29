namespace Notes.Api;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Notes.Api.Admin;
using Notes.Api.Database;

public class Startup
{
    private const string AllowSpecificOrigins = "AllowSpecificOrigins";

    private readonly OpenApiInfo _apiInfo = new OpenApiInfo
    {
        Version = "v1",
        Title = "Notes API",
        Description = "An API for sticky notes.",
    };

    private Secrets _secrets;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var secrets = Configuration.GetSection("Secrets").Get<Secrets>();
        services.AddSingleton(secrets);
        Secret.Secrets = secrets;
        _secrets = secrets;

        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddSwaggerGenNewtonsoftSupport();

        services.AddCors(
            options => options.AddPolicy(
                AllowSpecificOrigins,
                builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()));

        services
            .AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Description = "Basic auth added to authorization header",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "basic"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
                    },
                    new List<string>()
                }
            });

            options.SwaggerDoc(_apiInfo.Version, _apiInfo);
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        var connectionString = Configuration.GetConnectionString("NotesDb");
        services.AddDbContext<NotesDb>(options => options.UseSqlite("Data Source=notes.db"));
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment, NotesDb database)
    {
        if (_secrets.SeedData)
        {
            database.Database.EnsureDeleted();
            database.Database.EnsureCreated();
        }

        application
            .UseDeveloperExceptionPage()
            .UseCors(AllowSpecificOrigins)
            .UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "../Notes.Client")),
                RequestPath = "/client",
                EnableDirectoryBrowsing = true
            })
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", _apiInfo.Title);
            })
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}