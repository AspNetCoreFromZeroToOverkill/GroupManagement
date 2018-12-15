using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly GroupManagementDbContext _context;

        public GroupsService(GroupManagementDbContext context)
        {
            _context = context;
        }
        
        public async Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken ct)
        {
            var groups = await _context.Groups.AsNoTracking().OrderBy(g => g.Id).ToListAsync(ct);
            return groups.ToService();
        }

        public async Task<Group> GetByIdAsync(long id, CancellationToken ct)
        {
            var group = await _context.Groups.AsNoTracking().SingleOrDefaultAsync(g => g.Id == id, ct);
            return group.ToService();
        }

        public async Task<Group> UpdateAsync(Group group, CancellationToken ct)
        {
            var updatedGroupEntry = _context.Groups.Update(group.ToEntity());
            await _context.SaveChangesAsync(ct);
            return updatedGroupEntry.Entity.ToService();
        }
        

        public async Task<Group> AddAsync(Group group, CancellationToken ct)
        {
            var addedGroupEntry = _context.Groups.Add(group.ToEntity());
            await _context.SaveChangesAsync(ct);
            return addedGroupEntry.Entity.ToService();
        }

        public async Task RemoveAsync(long id, CancellationToken ct)
        {
            var group = await _context.Groups.SingleOrDefaultAsync(g => g.Id == id, ct);
            if(group != null)
            {
                _context.Remove(group);
                await _context.SaveChangesAsync(ct);
            }            
        }
    }
}