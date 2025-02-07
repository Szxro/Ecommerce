using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InjectAttribute : Attribute
{
    public InjectAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = ServiceLifetime;
    }
    public ServiceLifetime ServiceLifetime { get; set; }
}