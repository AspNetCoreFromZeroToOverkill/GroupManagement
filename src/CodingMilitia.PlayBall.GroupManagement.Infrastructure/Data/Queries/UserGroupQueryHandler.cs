using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries
{
    public class UserGroupQueryHandler : IQueryHandler<UserGroupQuery, Group>
    {
        private readonly GroupManagementDbContext _db;

        public UserGroupQueryHandler(GroupManagementDbContext db)
        {
            _db = db;
        }

        public async Task<Group> HandleAsync(UserGroupQuery query, CancellationToken ct)
            => await _db
                .Groups
                .Include(g => g.Creator)
                .Include(g => g.GroupUsers)
                .ThenInclude(g => g.User)
                .SingleOrDefaultAsync(g => g.Id == query.GroupId && g.GroupUsers.Any(gu => gu.User.Id == query.UserId), ct);
    }
}