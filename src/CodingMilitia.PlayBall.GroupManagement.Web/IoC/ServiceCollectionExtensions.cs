using System;
using System.Security.Claims;
using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Configuration;
using CodingMilitia.PlayBall.GroupManagement.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequiredMvcComponents(this IServiceCollection services)
        {
            services.AddTransient<ApiExceptionFilter>();

            var mvcBuilder = services.AddMvcCore(options =>
            {
                options.Filters.AddService<ApiExceptionFilter>();
                
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("scope", "GroupManagement")
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);   
            mvcBuilder.AddJsonFormatters();
            mvcBuilder.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddConfiguredAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    var authServiceConfig = configuration.GetSection<AuthServiceSettings>(nameof(AuthServiceSettings));

                    options.Authority = authServiceConfig.Authority;
                    options.RequireHttpsMetadata = authServiceConfig.RequireHttpsMetadata;
                    options.Audience = "GroupManagement";
                });

            return services;
        }
        
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IGroupsService, GroupsService>();
            
            //more business services...

            return services;
        }
        
        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
 
            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}