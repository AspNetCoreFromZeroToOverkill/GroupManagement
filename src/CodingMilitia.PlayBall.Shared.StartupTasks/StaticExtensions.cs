using CodingMilitia.PlayBall.Shared.StartupTasks;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StaticExtensions
    {
        public static IServiceCollection AddDbInitializer<T> (this IServiceCollection services) where T : DbContext
        {
            return services.AddAsyncInitializer<DbInitializer<T>>();
        }
    }
}