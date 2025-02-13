using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface ITokenService
{
    string GenerateToken(User user,double lifeTime = 10.00);

    string GenerateRefreshToken(int length = 32);
}
