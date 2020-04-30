using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data
{
    public delegate Task<TQueryResult> QueryRunner<in TQuery, TQueryResult>(TQuery query, CancellationToken ct);
}