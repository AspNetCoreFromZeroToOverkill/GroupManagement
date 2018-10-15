using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddSingleton<IGroupsService, InMemoryGroupsService>();
            
            //more business services...

            return services;
        }
    }
}