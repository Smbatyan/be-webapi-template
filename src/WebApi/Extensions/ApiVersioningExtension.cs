using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace WebApi.Extensions;

internal static class ApiVersioningExtension
{
    internal static IServiceCollection AddApiVersioningFromHeader(this IServiceCollection services)
    {
        return services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;

            opt.AssumeDefaultVersionWhenUnspecified = true;

            opt.DefaultApiVersion = new ApiVersion(1, 0);

            opt.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("panda-api-version"));
        });
    }
}