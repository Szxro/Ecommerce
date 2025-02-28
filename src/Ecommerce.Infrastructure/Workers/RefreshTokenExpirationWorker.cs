using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Workers;

public class RefreshTokenExpirationWorker : BaseWorker<RefreshTokenExpirationWorker>
{
    private readonly IServiceScopeFactory _scopeFactory;

    private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(50);

    public RefreshTokenExpirationWorker(
        ILogger<BaseWorker<RefreshTokenExpirationWorker>> logger,
        IServiceScopeFactory scopeFactory
        ) : base(logger)
    {
        _scopeFactory = scopeFactory;
    }

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(Timeout);

        while (await timer.WaitForNextTickAsync(cancellationToken)
            && !cancellationToken.IsCancellationRequested)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ExpirationStrategyService<RefreshToken> strategyService = scope.ServiceProvider.GetRequiredService<ExpirationStrategyService<RefreshToken>>();

            await strategyService.ExecuteAsync(DateTime.UtcNow, cancellationToken);
        }
    }
}
