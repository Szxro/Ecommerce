using Ecommerce.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<DatabaseInitializerWorker>();

        return services;
    }
}
