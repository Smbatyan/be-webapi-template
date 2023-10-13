
namespace WebApi.Extensions;

public static class AppSettingsExtensions
{
    public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}