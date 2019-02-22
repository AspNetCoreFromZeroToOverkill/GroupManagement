using AspNetCore.AsyncInitialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.Shared.StartupTasks
{
    public class DbInitializer<T> : IAsyncInitializer where T : DbContext
    {
        private IHostingEnvironment _hostingEnvironment;
        private DbContext _dbContext;

        public DbInitializer(T dbContext, IHostingEnvironment host)
        {
            _hostingEnvironment = host;
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            //TODO: Pass hardcoded values to the constructor - https://github.com/AspNetCoreFromZeroToOverkill/GroupManagement/issues/26
            if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsEnvironment("DockerDevelopment")) 
            {
                await _dbContext.Database.MigrateAsync();
            }
        }
    }
}
