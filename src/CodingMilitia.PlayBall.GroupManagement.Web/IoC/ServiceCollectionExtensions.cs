using System;
using System.Linq;
using System.Reflection;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data;
using CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Web.Configuration;
using CodingMilitia.PlayBall.GroupManagement.Web.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using UseCases = CodingMilitia.PlayBall.GroupManagement.Domain.UseCases;

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
                typeof(UseCases.GetUserGroups.GetUserGroupsQuery));

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddSingleton<ICurrentUserAccessor, CurrentUserAccessor>()
                .AddScoped(typeof(EfRepository<>))
                .Scan(scan =>
                    scan
                        .FromAssembliesOf(typeof(GroupManagementDbContext))
                        .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime()
                        .AddClasses(classes => classes.AssignableTo(typeof(IVersionedRepository<,>)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime()
                        .AddClasses(classes => classes.AssignableTo(typeof(IQueryRunner<,>)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime()
                )
                .AddRegisteredQueryRunnersAsDelegates();
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

        private static IServiceCollection AddRegisteredQueryRunnersAsDelegates(this IServiceCollection services)
        {
            var registerMethod = typeof(ServiceCollectionExtensions)
                .GetMethod(nameof(RegisterQueryRunnerAsDelegate), BindingFlags.Static | BindingFlags.NonPublic);

            var queryRunnerTypes = services
                .Where(descriptor => 
                        descriptor.ServiceType.IsGenericType 
                        && descriptor.ServiceType.GetGenericTypeDefinition() == typeof(IQueryRunner<,>))
                .ToList();

            foreach (var queryRunnerType in queryRunnerTypes)
            {
                var parameterizedRegisterMethod = registerMethod!.MakeGenericMethod(queryRunnerType.ServiceType.GenericTypeArguments);
                parameterizedRegisterMethod.Invoke(null, new object[] {services});
            }

            return services;
        }
        
        private static void RegisterQueryRunnerAsDelegate<TQuery, TQueryResult>(IServiceCollection services)
            where TQuery : IQuery<TQueryResult>
        {
            services.AddScoped<QueryRunner<TQuery,TQueryResult>>(
                provider => provider.GetRequiredService<IQueryRunner<TQuery, TQueryResult>>().RunAsync);
        }

    }
}