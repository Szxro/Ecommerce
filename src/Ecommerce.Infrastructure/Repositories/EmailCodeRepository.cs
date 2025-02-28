using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class EmailCodeRepository : GenericRepository<EmailCode>, IEmailCodeRepository
{
    public EmailCodeRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<EmailCode?> GetCurrentActiveEmailCodeByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.EmailCode
                                  .Where(x => x.User.Username.Value == username && !x.IsUsed && !x.IsExpired && !x.IsRevoked)
                                  .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<EmailCode?> GetEmailCodeByEmailCodeAsync(string emailCode,CancellationToken cancellationToken = default)
    {
        return await _appDbContext.EmailCode
                                  .Include(x => x.User)
                                  .FirstOrDefaultAsync(x => x.Code == emailCode,cancellationToken);
    }

    public async Task<bool> IsUserEmailCodeUsedByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.EmailCode
                                  .AsNoTracking()
                                  .AnyAsync(x => x.User.Username.Value == username && x.IsUsed, cancellationToken);
    }
}
