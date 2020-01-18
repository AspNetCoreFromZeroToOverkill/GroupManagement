using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data
{
    public class EfVersionedRepository<TEntity, TVersion> : IVersionedRepository<TEntity, TVersion>
        where TEntity : class, IVersionedEntity<TVersion>
        where TVersion : IComparable<TVersion>
    {
        private readonly EfRepository<TEntity> _repository;

        public EfVersionedRepository(EfRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
            => _repository.AddAsync(entity, ct);

        public async Task UpdateAsync(TEntity entity, TVersion knownVersion, CancellationToken ct)
        {
            if (entity.RowVersion.CompareTo(knownVersion) != 0)
            {
                // TODO: use a better error strategy
                throw new DbUpdateConcurrencyException();
            }

            await _repository.UpdateAsync(entity, ct);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken ct)
            => _repository.DeleteAsync(entity, ct);
    }
}