using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Services;
using Ecommerce.Infrastructure.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddStrategies(this IServiceCollection services)
    {
        // Context
        services.AddScoped<ExpirationStrategyService<EmailCode>>();

        services.AddScoped<ExpirationStrategyService<RefreshToken>>();

        // Strategies
        services.AddScoped<IExpiredStrategy<EmailCode>, EmailCodeExpirationStrategy>();

        services.AddScoped<IExpiredStrategy<RefreshToken>, RefreshTokenExpirationStrategy>();

        return services;
    }
}
