using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IRefreshTokenRepository : IRepositoryWriter<RefreshToken>
{
    Task<RefreshToken?> GetUnusedUserRefreshTokenByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetValidRefreshTokenAsync(string refreshToken ,string username, CancellationToken cancellationToken = default);
}
