using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class CredentialsRepository
    : GenericRepository<Credentials>, ICredentialsRepository
{
    public CredentialsRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Credentials?> GetCredentialsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Credentials
                                  .Where(x => x.User.Username.Value == username && x.IsActive)
                                  .FirstOrDefaultAsync(cancellationToken);
    }
}
