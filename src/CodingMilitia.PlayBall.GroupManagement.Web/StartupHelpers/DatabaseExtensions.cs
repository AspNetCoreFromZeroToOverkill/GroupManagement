using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
    internal static class DatabaseExtensions
    {
        internal static async Task EnsureDbUpToDateAsync(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                if (hostingEnvironment.IsDevelopment() || hostingEnvironment.IsEnvironment("DockerDevelopment"))
                {
                    var context = scope.ServiceProvider.GetService<GroupManagementDbContext>();
                    await context.Database.MigrateAsync();
                }
            }
        }
    }
}