using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries
{
    public class UserByIdQueryRunner : IQueryRunner<UserByIdQuery, Optional<User>>
    {
        private readonly GroupManagementDbContext _db;

        public UserByIdQueryRunner(GroupManagementDbContext db)
        {
            _db = db;
        }

        public async Task<Optional<User>> RunAsync(UserByIdQuery query, CancellationToken ct)
            => Optional.FromNullable(await _db.Set<User>().FindAsync(new object[] {query.UserId}, ct));
    }
}