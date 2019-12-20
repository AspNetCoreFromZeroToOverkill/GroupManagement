using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries
{
    public class UserByIdQueryHandler : IQueryHandler<UserByIdQuery, User>
    {
        private readonly GroupManagementDbContext _db;

        public UserByIdQueryHandler(GroupManagementDbContext db)
        {
            _db = db;
        }

        public async Task<User> HandleAsync(UserByIdQuery query, CancellationToken ct)
            => await _db.Set<User>().FindAsync(new object[] {query.UserId}, ct);
    }
}