using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Workers;

public class DatabaseInitializerWorker : BaseWorker<DatabaseInitializerWorker>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DatabaseInitializerWorker(
        ILogger<BaseWorker<DatabaseInitializerWorker>> logger,
        IServiceScopeFactory scopeFactory) : base(logger)
    {
        _scopeFactory = scopeFactory;
    }

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _scopeFactory.CreateScope(); ;

        IDatabaseServiceInitializer initializerService = scope.ServiceProvider.GetRequiredService<IDatabaseServiceInitializer>();

        await initializerService.CanConnectAsync(cancellationToken);

        await initializerService.MigrateAsync(cancellationToken);
    }
}
