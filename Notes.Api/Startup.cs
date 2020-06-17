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
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
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
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
                });

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
            if (connectionString.Contains('<'))
            {
                services.AddDbContext<NotesDb>(options => options.UseSqlite("Data Source=notes.db"));
            }
            else
            {
                services.AddDbContext<NotesDb>(options => options.UseSqlServer(connectionString));
            }
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment, NotesDb database)
        {
            database.Database.EnsureDeleted();
            database.Database.EnsureCreated();

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
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
