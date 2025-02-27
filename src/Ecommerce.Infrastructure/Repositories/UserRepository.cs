using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class UserRepository 
    : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.User
                                  .Include(x => x.Credentials)
                                  .Where(x => x.Username.Value == username).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsEmailNotUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return _appDbContext.User.AsNoTracking().AnyAsync(x => x.Email.Value == email, cancellationToken);
    }

    public Task<bool> IsUsernameNotUniqueAsync(string username, CancellationToken cancellationToken = default)
    {
        return _appDbContext.User.AsNoTracking().AnyAsync(x => x.Username.Value == username, cancellationToken);
    }
}
