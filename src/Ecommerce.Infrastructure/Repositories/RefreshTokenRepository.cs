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

    public async Task<RefreshToken?> GetUnusedUserRefreshTokenByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.RefreshToken
                                  .Where(x => x.User.Username.Value == username)
                                  .FirstOrDefaultAsync(cancellationToken);
    }
}
