using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Queries
{
    public class UserGroupsQueryHandler : IQueryHandler<UserGroupsQuery, IReadOnlyCollection<Group>>
    {
        private readonly GroupManagementDbContext _db;

        public UserGroupsQueryHandler(GroupManagementDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyCollection<Group>> HandleAsync(UserGroupsQuery query, CancellationToken ct)
            => (await _db
                .Groups
                .Include(g => g.Creator)
                .Where(g => g.GroupUsers.Any(gu => gu.User.Id == query.UserId))
                .ToListAsync(ct))
                .AsReadOnly();
    }
}