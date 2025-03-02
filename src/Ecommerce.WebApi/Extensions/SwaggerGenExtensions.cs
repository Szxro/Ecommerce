using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Ecommerce.WebApi.Extensions;

public static partial class ApiExtensions
{
    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            OpenApiSecurityScheme jwtScheme = new OpenApiSecurityScheme
            {
                Name = "Jwt Authentication",
                Description = "Standard Authorization header using the Bearer Scheme (\"Bearer {token}\")",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "Jwt"
            };

            OpenApiSecurityRequirement jwtRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            options.AddSecurityRequirement(jwtRequirement);

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtScheme);
        });

        return services;
    }
}
