using Ecommerce.SharedKernel.Contracts;
using System.Linq.Expressions;

namespace Ecommerce.Domain.Contracts;

public interface IRepositoryRemover<TEntity>
    where TEntity : IEntity
{
    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    Task<int?> RemoveById(int id, CancellationToken cancellationToken = default);

    Task<int?> BulkDelete(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
}
