using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters
{
    public class DemoExceptionFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<DemoExceptionFilter>();
            filter.Suffix = $"by {nameof(DemoExceptionFilterFactoryAttribute)}";
            return filter;
        }

        public bool IsReusable { get; } = false;
    }
}