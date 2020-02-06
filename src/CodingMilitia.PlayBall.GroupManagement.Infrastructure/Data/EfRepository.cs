using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data
{
    // based on https://github.com/dotnet-architecture/eShopOnWeb/blob/netcore2.2/src/Infrastructure/Data/EfRepository.cs
    public class EfRepository<TEntity> where TEntity : class
    {
        private readonly GroupManagementDbContext _dbContext;

        public EfRepository(GroupManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken ct)
        {
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken ct)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}