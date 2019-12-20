using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity, CancellationToken ct);
        Task UpdateAsync(T entity, CancellationToken ct);
        Task DeleteAsync(T entity, CancellationToken ct);
    }
}