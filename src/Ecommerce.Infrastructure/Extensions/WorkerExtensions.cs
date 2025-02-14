using Ecommerce.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<DatabaseInitializerWorker>();

        services.AddHostedService<DomainEventDispatcherWorker>();

        //services.AddHostedService<EmailCodeExpirationWorker>();

        //services.AddHostedService<RefreshTokenExpirationWorker>();

        return services;
    }
}
