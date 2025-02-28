using Ecommerce.WebApi.ExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ecommerce.WebApi.Extensions;

public static partial class ApiExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IHostEnvironment environment)
    {
        // Authorization and Authorization
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddAuthorization();

        // Core
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGenWithAuth();
        services.AddProblemDetails();

        // Exception Handlers
        services.AddExceptionHandler<GlobalExceptionHandler>();

        // Cors
        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                policy.SetIsOriginAllowed(origin => true);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();                                
            });
        });

        return services;
    }
}
