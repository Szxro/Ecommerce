using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Strategies;

public class RefreshTokenExpirationStrategy : IExpiredStrategy<RefreshToken>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenExpirationStrategy(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<List<RefreshToken>> GetExpiredEntitiesAsync(
        DateTime currentDateTime,
        CancellationToken cancellationToken = default)
    {
        return await _refreshTokenRepository.GetExpiredRefreshTokensAsync(currentDateTime, cancellationToken);
    }

    public void MarkEntitiesAsExpired(List<RefreshToken> entities)
    {
        foreach (RefreshToken entity in entities)
        {
            entity.IsExpired = true;
        }
    }
}
