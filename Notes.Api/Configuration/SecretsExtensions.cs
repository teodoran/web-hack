namespace Notes.Api.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Api.Admin;

public static class SecretsExtensions
{
    public static IServiceCollection ConfigureSecrets(this IServiceCollection services, IConfiguration configuration)
    {
        var secretsSection = configuration.GetSection("Secrets");
        services.Configure<Secrets>(secretsSection);
        Secret.Secrets = secretsSection.Get<Secrets>();

        return services;
    }
}