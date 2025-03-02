using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerce.Infrastructure.Options.Jwt;

public class JwtBearerTokenOptions : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;
    private readonly IHostEnvironment _hostEnvironment;

    public JwtBearerTokenOptions(
        IOptions<JwtOptions> options,
        IHostEnvironment hostEnvironment)
    {
        _jwtOptions = options.Value;
        _hostEnvironment = hostEnvironment;
    }

    // These configuration run after all IConfigureOptions<TOptions> (singleton)
    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        // Ony disable it in a development environment
        if (_hostEnvironment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = _jwtOptions.ValidateAudience,
            ValidateIssuer = _jwtOptions.ValidateIssuer,
            ValidateLifetime = _jwtOptions.ValidateLifetime,
            ValidAudience = _jwtOptions.ValidAudience,
            ValidIssuer = _jwtOptions.ValidIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            ClockSkew = TimeSpan.Zero // must override (validating time) (default one is 5 min)
        };
    }
}
