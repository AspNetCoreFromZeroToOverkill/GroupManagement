using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data
{
    public interface IVersionedRepository<TEntity, TVersion> 
        where TEntity : IVersionedEntity<TVersion>
        where TVersion : IComparable<TVersion>
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken ct);
        Task UpdateAsync(TEntity entity, TVersion knownVersion, CancellationToken ct);
        Task DeleteAsync(TEntity entity, CancellationToken ct);
    }
}