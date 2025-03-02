using Ecommerce.Domain.Entities;
using System.Security.Claims;

namespace Ecommerce.Domain.Contracts;

public interface ITokenService
{
    string GenerateToken(User user,double lifeTime = 10.00);

    string GenerateRefreshToken(int length = 32);

    Task<(bool isValid, ClaimsIdentity? claims)> ValidateToken(string token);
}
