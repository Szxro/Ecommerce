using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Strategies;

public class RefreshTokenExpirationStrategy : IExpiredStrategy<RefreshToken>
{
    public RefreshTokenExpirationStrategy()
    {
        
    }

    public Task<List<RefreshToken>> GetExpiredEntitiesAsync(
        DateTime currentDateTime,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void MarkEntitiesAsExpired(List<RefreshToken> entities)
    {
        throw new NotImplementedException();
    }
}
