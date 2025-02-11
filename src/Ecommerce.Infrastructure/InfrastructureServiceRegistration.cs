using Ecommerce.Infrastructure.Extensions;
using Ecommerce.Infrastructure.Options.Database;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Persistence.Interceptors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IHostEnvironment environment)
    {
        services.AddValidatorsFromAssembly(typeof(InfrastructureServiceRegistration).Assembly);

        services
            .AddWorkers()
            .AddStrategies()
            .AddConfigurableOptions();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            DatabaseOptions databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            options.UseSqlServer(databaseOptions.ConnectionString,options =>
            {
                options.CommandTimeout(databaseOptions.CommandTimeout);            
            })
            .AddInterceptors(provider.GetRequiredService<AuditableEntityInterceptor>())
            .UseSnakeCaseNamingConvention();

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
                options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            }
        });

        services.RegisterServicesFromAssembly(typeof(InfrastructureServiceRegistration).Assembly);

        return services;
    }
}
