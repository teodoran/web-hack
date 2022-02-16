namespace Notes.Api.Configuration;

using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Primitives;

public static class CorrelationIdExtensions
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder application) =>
        application.Use(async (context, next) =>
        {
            var headers = context.Request.Headers;
            if (headers.TryGetValue(CorrelationIdHeader, out StringValues correlationId))
            {
                Activity.Current.AddBaggage(CorrelationIdHeader, correlationId.FirstOrDefault());
            }

            await next.Invoke();
        });
}