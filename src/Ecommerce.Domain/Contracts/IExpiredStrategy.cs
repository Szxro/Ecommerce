using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Contracts;

public interface IExpiredStrategy<TEntity>
    where TEntity : Entity
{
    Task<List<TEntity>> GetExpiredEntitiesAsync(DateTime currentDateTime, CancellationToken cancellationToken = default);

    void MarkEntitiesAsExpired(List<TEntity> entities);
}