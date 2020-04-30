using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries
{
    public interface IQueryRunner<in TQuery, TQueryResult> where TQuery : IQuery<TQueryResult>
    {
        Task<TQueryResult> RunAsync(TQuery query, CancellationToken ct);
    }
}