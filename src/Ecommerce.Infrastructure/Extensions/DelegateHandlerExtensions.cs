using Ecommerce.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Extensions;
public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddHttpHandlers(this IServiceCollection services)
    {       
        services.AddTransient<RetryHandler>(); // Need to be register as transient (Custom Delegate Handler)

        services.AddTransient<LoggingHandler>();

        return services;
    }
}
