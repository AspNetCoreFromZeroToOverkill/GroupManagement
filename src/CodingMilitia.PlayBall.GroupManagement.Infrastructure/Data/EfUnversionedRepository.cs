using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data
{
    public class EfUnversionedRepository<TEntity> : IRepository<TEntity> where TEntity : class, IUnversionedEntity
    {
        private readonly EfRepository<TEntity> _repository;

        public EfUnversionedRepository(EfRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
            => _repository.AddAsync(entity, ct);

        public Task UpdateAsync(TEntity entity, CancellationToken ct)
            => _repository.UpdateAsync(entity, ct);

        public Task DeleteAsync(TEntity entity, CancellationToken ct)
            => _repository.DeleteAsync(entity, ct);
    }
}