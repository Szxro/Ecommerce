using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Options.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.Infrastructure.Services;

[Inject(ServiceLifetime.Transient)]
public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly JwtOptions _jwtOptions;

    private static readonly string Algorithm = SecurityAlgorithms.HmacSha256;
    private static readonly string JwtClaimEmailVerified = "is_email_verified";
    private static readonly string JwtClaimIsAdministrator = "is_admin";

    public TokenService(
        ILogger<TokenService> logger,
        IOptions<JwtOptions> options)
    {
        _logger = logger;
        _jwtOptions = options.Value;
    }

    public string GenerateToken(User user, double lifeTime = 10)
    {
        // Its more faster than JwtSecurityTokenHandler 
        JsonWebTokenHandler handler = new JsonWebTokenHandler();

        try
        {
            SigningCredentials credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), Algorithm);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.Now.AddMinutes(lifeTime),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.ValidIssuer,
                Audience = _jwtOptions.ValidAudience
            };

            string token = handler.CreateToken(tokenDescriptor);

            return token;

        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An unexpected error happen while trying to create a token for a user with the error message : '{message}'",
                ex.Message);

            throw;
        }
    }

    public string GenerateRefreshToken(int length = 32)
    {
        byte[] buffer = new byte[length];

        using RandomNumberGenerator rng = RandomNumberGenerator.Create();

        // Fill the buffer with random sequences of values
        rng.GetBytes(buffer);

        // Converting the buffer to its representation in base 64 string
        string refreshToken = Convert.ToBase64String(buffer);

        return refreshToken;
    }

    private ClaimsIdentity GenerateClaims(User user)
    {
        ClaimsIdentity claims = new ClaimsIdentity(
            new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,$"{user.Id}"),
            new Claim(JwtRegisteredClaimNames.Name,user.Username.Value),
            new Claim(JwtRegisteredClaimNames.Email,user.Email.Value),
            new Claim(JwtClaimEmailVerified,user.EmailCodes.Any(x => x.IsUsed) ? "true" : "false"),
            new Claim(JwtClaimIsAdministrator,user.IsAdmin ? "true" : "false")
        });

        return claims;
    }
}
