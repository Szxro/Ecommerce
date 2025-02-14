using Ecommerce.Infrastructure.Options.Database;
using Ecommerce.Infrastructure.Options.Hash;
using Ecommerce.Infrastructure.Options.Jwt;
using Ecommerce.Infrastructure.Options.Smtp;
using Ecommerce.Infrastructure.Options.Validator;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddConfigurableOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>()
                .AddFluentValidator<DatabaseOptions>();

        services.ConfigureOptions<JwtOptionsSetup>()
                .AddFluentValidator<JwtOptions>();

        services.ConfigureOptions<JwtBearerTokenOptions>();

        services.ConfigureOptions<HashOptionsSetup>()
                .AddFluentValidator<HashOptions>();

        services.ConfigureOptions<SmtpServerOptionsSetup>()
                .AddFluentValidator<SmtpServerOptions>();

        return services;
    }

    private static IServiceCollection AddFluentValidator<TOption>(this IServiceCollection services)
      where TOption : class, IConfigurationOptions
    {
        services.AddSingleton<IValidateOptions<TOption>>(
            provider => new FluentOptionsValidator<TOption>(provider.GetRequiredService<IServiceScopeFactory>()));

        return services;
    }
}