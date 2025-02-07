using Ecommerce.Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<AuditableEntityInterceptor>();

        return services;
    }
}
