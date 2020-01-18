using System;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data;
using CodingMilitia.PlayBall.GroupManagement.Web.Configuration;
using CodingMilitia.PlayBall.GroupManagement.Web.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace - for better discoverability
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequiredMvcComponents(this IServiceCollection services)
        {
            services.AddTransient<ApiExceptionFilter>();

            services.AddControllers(options => { options.Filters.AddService<ApiExceptionFilter>(); });

            return services;
        }

        public static IServiceCollection AddConfiguredAuthentication(this IServiceCollection services,
            IConfiguration configuration)
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

        public static IServiceCollection AddConfiguredAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("scope", "GroupManagement")
                    .Build();

                options.DefaultPolicy = policy;
            });

            return services;
        }

        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddMediatR(
                typeof(CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroups.GetUserGroupsQuery));

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped(typeof(EfRepository<>));
            services.Scan(scan =>
                scan
                    .FromAssembliesOf(typeof(GroupManagementDbContext))
                    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IVersionedRepository<,>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                );
            
            // without Scrutor (or an alternative) we'd need to do everything by hand, like:
            /*
                services.AddScoped<IRepository<Group, long>, EfRepository<Group, long>>();
                services.AddScoped<IRepository<User, string>, EfRepository<User, string>>();
                services.AddScoped<IQueryHandler<UserGroupsQuery, IReadOnlyCollection<Group>>, UserGroupsQueryHandler>();
                services.AddScoped<IQueryHandler<UserGroupQuery, Group>, UserGroupQueryHandler>();
                // ...
            */
            return services;
        }

        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration)
            where TConfig : class, new()
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