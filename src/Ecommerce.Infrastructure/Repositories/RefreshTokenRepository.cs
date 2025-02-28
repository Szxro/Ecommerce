using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class RefreshTokenRepository 
    : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<List<RefreshToken>> GetExpiredRefreshTokensAsync(DateTime currentTime, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.RefreshToken
                                  .Where(x => x.ExpirationDateAtUtc <= currentTime && !x.IsExpired && !x.IsUsed && !x.IsRevoked)
                                  .ToListAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetUnusedUserRefreshTokenByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.RefreshToken
                                  .Where(x => x.User.Username.Value == username && !x.IsUsed && !x.IsExpired && !x.IsRevoked)
                                  .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetValidRefreshTokenAsync(string refreshToken, string username,CancellationToken cancellationToken = default)
    {
        return await _appDbContext.RefreshToken
                                  .Include(x => x.User)
                                  .Where(x => x.User.Username.Value == username && x.Token == refreshToken && !x.IsExpired && !x.IsRevoked && !x.IsUsed)
                                  .FirstOrDefaultAsync(cancellationToken);
    }
}
