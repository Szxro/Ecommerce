using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InjectAttribute : Attribute
{
    public InjectAttribute(ServiceLifetime serviceLifetime) // Default value of ServiceLifetime is Singleton
    {
        ServiceLifetime = serviceLifetime;
    }
    public ServiceLifetime ServiceLifetime { get; }
}