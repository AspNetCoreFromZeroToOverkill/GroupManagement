using CodingMilitia.PlayBall.Shared.StartupTasks.DatabaseInitializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace - to ease discoverability
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseInitializerServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseInitializer<TDbContext>(
            this IServiceCollection services,
            string databaseInitializerSettingsPath = nameof(DatabaseInitializerSettings))
            where TDbContext : DbContext
        {
            services.AddSingleton(serviceProvider
                => serviceProvider
                    .GetRequiredService<IConfiguration>()
                    .GetSection(databaseInitializerSettingsPath)
                    .Get<DatabaseInitializerSettings>());

            return services.AddHostedService<DatabaseInitializerHostedService<TDbContext>>();
        }
    }
}