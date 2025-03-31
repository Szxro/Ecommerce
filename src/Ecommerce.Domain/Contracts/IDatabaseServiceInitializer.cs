namespace Ecommerce.Domain.Contracts;

public interface IDatabaseServiceInitializer
{
    Task MigrateAsync(CancellationToken cancellationToken = default);

    Task CanConnectAsync(CancellationToken cancellationToken = default);

    Task SeedAsync(CancellationToken cancellationToken = default);
}
