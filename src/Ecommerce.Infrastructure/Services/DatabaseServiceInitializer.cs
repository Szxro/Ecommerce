using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Services;

[Inject(ServiceLifetime.Scoped)]
public sealed class DatabaseServiceInitializer : IDatabaseServiceInitializer
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DatabaseServiceInitializer> _logger;

    public DatabaseServiceInitializer(
        AppDbContext appDbContext,
        ILogger<DatabaseServiceInitializer> logger)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }

    public async Task CanConnectAsync(CancellationToken cancellationToken = default)
    {
        bool isConnected = await _appDbContext.Database.CanConnectAsync(cancellationToken);

        if (!isConnected)
        {
            _logger.LogWarning("Cant connect to the database, database not available");

            throw new InvalidOperationException();
        }

        _logger.LogInformation("Successfully connect to the database!!!.");
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _appDbContext.Database.MigrateAsync(cancellationToken);

            _logger.LogInformation("Successfully apply the migrations!!.");

        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An error happen trying to apply migrations to the database {providerName} with the error message: {message}",
                _appDbContext.Database.ProviderName,
                ex.Message);

            throw;
        }
    }
}
