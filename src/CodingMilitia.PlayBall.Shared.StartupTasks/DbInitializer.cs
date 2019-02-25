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
        private string _environment;

        public DbInitializer(T dbContext, IHostingEnvironment host, string environment)
        {
            _hostingEnvironment = host;
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task InitializeAsync()
        {
            if (_hostingEnvironment.IsEnvironment(_environment)) 
            {
                await _dbContext.Database.MigrateAsync();
            }
        }
    }
}
