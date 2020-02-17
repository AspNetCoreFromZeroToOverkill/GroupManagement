using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries
{
    public class UserGroupQueryHandler : IQueryHandler<UserGroupQuery, Optional<Group>>
    {
        private readonly GroupManagementDbContext _db;

        public UserGroupQueryHandler(GroupManagementDbContext db)
        {
            _db = db;
        }

        public async Task<Optional<Group>> HandleAsync(UserGroupQuery query, CancellationToken ct)
            => Optional.FromNullable(await _db
                .Groups
                .Include(g => g.Creator)
                .Include(g => g.GroupUsers)
                .SingleOrDefaultAsync(g => g.Id == query.GroupId && g.GroupUsers.Any(gu => gu.UserId == query.UserId), ct));
    }
}