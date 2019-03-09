using CodingMilitia.PlayBall.GroupManagement.Data;
using CodingMilitia.PlayBall.Shared.StartupTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

[assembly: ApiController]
namespace CodingMilitia.PlayBall.GroupManagement.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRequiredMvcComponents();              

            services.AddDbContext<GroupManagementDbContext>(options =>
            {
                options.UseNpgsql(_config.GetConnectionString("GroupManagementDbContext"));
                options.EnableSensitiveDataLogging();
            });
            services.AddAsyncInitializer<DbInitializer<GroupManagementDbContext>>();
            services.AddBusiness();

            services
                .AddConfiguredAuth();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("X-Powered-By", "ASP.NET Core: From 0 to overkill");
                    return Task.CompletedTask;
                });

                await next.Invoke();
            });

            app.UseAuthentication();
            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("No middlewares could handle the request");
            });
        }
    }
}