using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Services;

public class ExpirationStrategyService<TEntity>
    where TEntity : Entity
{
    private readonly IExpiredStrategy<TEntity> _strategy;
    private readonly ILogger<ExpirationStrategyService<TEntity>> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ExpirationStrategyService(
        IExpiredStrategy<TEntity> strategy,
        ILogger<ExpirationStrategyService<TEntity>> logger,
        IUnitOfWork unitOfWork)
    {
        _strategy = strategy;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(
      DateTime dateTime,
      CancellationToken cancellationToken = default)
    {
        try
        {
            List<TEntity> expiredEntities = await _strategy.GetExpiredEntitiesAsync(dateTime, cancellationToken);

            if (expiredEntities.Count <= 0)
            {
                _logger.LogInformation("Not expired '{entityName}' entities found", typeof(TEntity).Name);

                return;
            }

            _logger.LogWarning(
                "Found {length} expired '{entityName}' entities, proceding to mark them as expired",
                expiredEntities.Count,
                typeof(TEntity).Name);

            _strategy.MarkEntitiesAsExpired(expiredEntities);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An unexpected error happen while processing expired {name} entitites  with the error message : '{message}'",
                typeof(TEntity).Name,
                ex.Message);
        }
    }
}
