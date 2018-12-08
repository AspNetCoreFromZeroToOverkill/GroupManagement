using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Services
{
    public interface IGroupsService
    {
        Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken ct);

        Task<Group> GetByIdAsync(long id, CancellationToken ct);

        Task<Group> UpdateAsync(Group group, CancellationToken ct);

        Task<Group> AddAsync(Group group, CancellationToken ct);
    }
}