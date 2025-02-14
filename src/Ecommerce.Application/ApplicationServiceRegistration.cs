using Ecommerce.Application.Common.Pipelines;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

            // Pipelines Behaviors
            options.AddOpenBehavior(typeof(RequestValidationPipelineBehavior<,>));
            options.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            options.AddOpenBehavior(typeof(RequestPerformancePipelineBehavior<,>));
            options.AddOpenBehavior(typeof(RequestTransactionHandlingBehavior<,>));
            options.AddOpenBehavior(typeof(RequestExceptionHandlingPipelineBehavior<,>));
            options.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
        });

        return services;
    }
}