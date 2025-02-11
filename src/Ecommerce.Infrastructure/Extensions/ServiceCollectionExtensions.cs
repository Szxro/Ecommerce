using Ecommerce.Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecommerce.Infrastructure.Extensions;

public static partial class InfrastructureExtensions
{
    /// <summary>
    /// Scans the specified assembly for classes marked with the <see cref="InjectAttribute"/> 
    /// and registers them in the service collection according to the lifetime defined in the attribute.
    /// If a class implements multiple interfaces, only the first one found is registered.
    /// If no interface is implemented, the class is not registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> where the discovered services will be registered.</param>
    /// <param name="assembly">The <see cref="Assembly"/> to scan for injectable services.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance containing the registered services.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid service lifetime is encountered.</exception>
    public static IServiceCollection RegisterServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<TypeInfo> types = assembly.DefinedTypes
                                              .Where(x => x.GetCustomAttribute<InjectAttribute>() is not null && !x.IsInterface && !x.IsAbstract && x.IsClass);

        foreach (TypeInfo type in types)
        {
            InjectAttribute? attribute = type.GetCustomAttribute<InjectAttribute>();

            // If the service dont have an interface , the service itself is going to be the serviceType
            Type interfaceType = type.GetInterfaces().FirstOrDefault() ?? type;

            if (attribute is null) continue;

            switch (attribute.ServiceLifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(interfaceType, type);
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton(interfaceType, type);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(interfaceType, type);
                    break;
                default:
                    throw new ArgumentException("Unknown Service Lifetime {serviceLifeTime}", nameof(attribute.ServiceLifetime));
            }
        }

        return services;
    }
}
