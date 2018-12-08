using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
    internal static class DatabaseExtensions
    {
        internal static async Task EnsureDbUpToDate(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GroupManagementDbContext>();
                await context.Database.MigrateAsync();
            }
        }
    }
}