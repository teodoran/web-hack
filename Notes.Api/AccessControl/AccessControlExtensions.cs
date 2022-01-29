namespace Notes.Api.AccessControl;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class AccessControlExtensions
{
    private const string AllowAllOrigins = "AllowAllOrigins";
    private const string BasicAuthentication = "BasicAuthentication";

    public static IServiceCollection AddAccessControl(this IServiceCollection services)
    {
        services
            .AddCors(options => options.AddPolicy(
                name: AllowAllOrigins,
                configurePolicy: builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()));

        services
            .AddAuthentication(BasicAuthentication)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication, null);

        return services;
    }

    public static IApplicationBuilder UseAccessControl(this IApplicationBuilder application) =>
        application
            .UseCors(AllowAllOrigins)
            .UseAuthentication()
            .UseAuthorization();
}