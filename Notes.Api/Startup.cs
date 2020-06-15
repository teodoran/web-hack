using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notes.Api.Database;

namespace Notes.Api
{
    public class Startup
    {
        private const string AllowSpecificOrigins = "AllowSpecificOrigins";

        private readonly OpenApiInfo _apiInfo = new OpenApiInfo
        {
            Version = "v1",
            Title = "Notes API",
            Description = "An API for sticky notes.",
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddControllers();
            services.AddCors(
                options => options.AddPolicy(
                    AllowSpecificOrigins,
                    builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_apiInfo.Version, _apiInfo);
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var connectionString = Configuration.GetConnectionString("ArkivDb");
            if (connectionString.Contains('<'))
            {
                services.AddDbContext<NotesDb>(options => options.UseInMemoryDatabase("Notes.Api.Database.InMemory"));
            }
            else
            {
                services.AddDbContext<NotesDb>(options => options.UseSqlServer(connectionString));
            }
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseHttpsRedirection();
            }

            application
                .UseCors(AllowSpecificOrigins)
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", _apiInfo.Title);
                })
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
