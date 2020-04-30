using System;
using System.Collections.Generic;
using System.Linq;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data;
using CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries;
using Xunit;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Tests.Data.Queries
{
    public class QueryRunnerTests
    {
        private static readonly Type QueryType = typeof(IQuery<>);
        private static readonly Type QueryRunnerType = typeof(IQueryRunner<,>);

        [Fact]
        public void AllDomainQueriesHaveMatchingInfrastructureRunnerTest()
        {
            var existingQueries = GetAllQueries();
            var existingQueryRunners = GetAllQueryRunners();

            Assert.All(
                existingQueries,
                query => existingQueryRunners.Single(runner => runner.GenericTypeArguments[0] == query));
        }
        
        private static IEnumerable<Type> GetAllQueries()
        {
            return QueryType.Assembly
                .GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().SingleOrDefault(IsQueryInterface) != null);

            static bool IsQueryInterface(Type @interface)
                => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == QueryType;
        }

        private static IEnumerable<Type> GetAllQueryRunners()
        {
            var infrastructureAssembly = typeof(GroupManagementDbContext).Assembly;

            return infrastructureAssembly
                .GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().SingleOrDefault(IsQueryRunnerInterface) != null)
                .Select(t => t.GetInterfaces().Single(IsQueryRunnerInterface));

            static bool IsQueryRunnerInterface(Type @interface)
                => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == QueryRunnerType;
        }
    }
}