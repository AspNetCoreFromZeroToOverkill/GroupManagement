using AspNetCore.AsyncInitialization;
using CodingMilitia.PlayBall.GroupManagement.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.Shared.StartupTasks
{
    public class DbInitializer : IAsyncInitializer
    {
        private IWebHost _host;
        public DbInitializer(IWebHost host)
        {
            this._host = host;
        }

        public async Task InitializeAsync()
        {
            using (var scope = _host.Services.CreateScope())
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
