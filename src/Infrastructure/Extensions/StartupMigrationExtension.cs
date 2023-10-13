using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

internal static class StartupMigrationExtension
{
    internal static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        PostgresContext context = serviceProvider.GetRequiredService<PostgresContext>();
        Console.WriteLine(context.Database.GetConnectionString());
        
        context.Database.Migrate();

        return services;
    }
}