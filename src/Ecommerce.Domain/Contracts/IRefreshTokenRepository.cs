using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IRefreshTokenRepository : IRepositoryWriter<RefreshToken>
{
    Task<RefreshToken?> GetUnusedUserRefreshTokenByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
