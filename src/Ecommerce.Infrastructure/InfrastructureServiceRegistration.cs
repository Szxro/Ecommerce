using Ecommerce.Infrastructure.Extensions;
using Ecommerce.Infrastructure.Handlers;
using Ecommerce.Infrastructure.Options.Database;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Persistence.Interceptors;
using Ecommerce.Infrastructure.Services;
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
            .AddConfigurableOptions()
            .AddInterceptors()
            .AddHttpHandlers();

        services.AddHttpClient<RestCountryService>(options =>
        {
            // Note: Can create an option class to get the url dynamically (in the params have a service provider)
            options.BaseAddress = new Uri("https://restcountries.com");
            // Note: The http client for strongly type are registered as transient services.

        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new SocketsHttpHandler
            {
                // Microsoft Recommendation: (how long a connection can be reusable?) 
                PooledConnectionLifetime = TimeSpan.FromMinutes(5)
                // Fix port exhaustion and reacting to dns changes
            };
        })
        .AddHttpMessageHandler<LoggingHandler>() // Need to be register in order of execution (act like middlewares)
        .AddHttpMessageHandler<RetryHandler>();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            DatabaseOptions databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            options.UseSqlServer(databaseOptions.ConnectionString, options =>
            {
                options.CommandTimeout(databaseOptions.CommandTimeout);
            })
            .AddInterceptors(
                provider.GetRequiredService<AuditableEntityInterceptor>(),
                provider.GetRequiredService<SoftDeleteInterceptor>()
             )
            .UseSnakeCaseNamingConvention();           

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
                options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            }
        });

        services.RegisterServicesFromAssembly(typeof(InfrastructureServiceRegistration).Assembly);

        services.AddHttpContextAccessor();

        services.AddMemoryCache(); // By Now is going to be memory cache (later distributed cache with redis)

        return services;
    }
}
