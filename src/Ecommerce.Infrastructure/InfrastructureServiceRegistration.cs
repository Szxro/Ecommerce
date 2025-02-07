using Ecommerce.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {


        services.RegisterServicesFromAssembly(typeof(InfrastructureServiceRegistration).Assembly);

        services.AddWorkers();

        return services;
    }
}
