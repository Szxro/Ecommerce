using System.Data;

namespace Ecommerce.Domain.Contracts;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<int?> SaveChangesAsync(CancellationToken cancellationToken = default);

    void ChangeTrackerToUnchanged<T>(T entity) where T : class;
}
