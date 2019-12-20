using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data
{
    // based on https://github.com/dotnet-architecture/eShopOnWeb/blob/netcore2.2/src/Infrastructure/Data/EfRepository.cs
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly GroupManagementDbContext _dbContext;

        public EfRepository(GroupManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<T> AddAsync(T entity, CancellationToken ct)
        {
            await _dbContext.Set<T>().AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken ct)
        {
            var entry = _dbContext.Entry(entity);
            if (entity is IVersionedEntity versionedEntity)
            {
                entry.OriginalValues[nameof(IVersionedEntity.RowVersion)] = versionedEntity.RowVersion;
            }
            entry.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(T entity, CancellationToken ct)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}