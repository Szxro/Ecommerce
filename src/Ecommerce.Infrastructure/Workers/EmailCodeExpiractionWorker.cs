using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Workers;

public class EmailCodeExpirationWorker : BaseWorker<EmailCodeExpirationWorker>
{
    private readonly IServiceScopeFactory _scopeFactory;

    // Get the expiration time or timeout from configuration?
    private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(35);

    public EmailCodeExpirationWorker(
        ILogger<BaseWorker<EmailCodeExpirationWorker>> logger,
        IServiceScopeFactory scopeFactory) : base(logger)
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

            ExpirationStrategyService<EmailCode> strategyService = scope.ServiceProvider.GetRequiredService<ExpirationStrategyService<EmailCode>>();

            await strategyService.ExecuteAsync(DateTime.Now, cancellationToken);
        }
    }
}
