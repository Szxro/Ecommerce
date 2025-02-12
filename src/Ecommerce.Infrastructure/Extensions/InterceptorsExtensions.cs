using Ecommerce.Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        // By default the interceptors have an interface call ISaveChangesInterceptor
        services.AddSingleton<AuditableEntityInterceptor>();

        return services;
    }
}
