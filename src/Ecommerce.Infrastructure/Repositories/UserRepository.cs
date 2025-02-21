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

    public Task<bool> IsEmailNotUnique(string email, CancellationToken cancellationToken = default)
    {
        return _appDbContext.User.AsNoTracking().AnyAsync(x => x.Email.Value == email, cancellationToken);
    }

    public Task<bool> IsUsernameNotUnique(string username, CancellationToken cancellationToken = default)
    {
        return _appDbContext.User.AsNoTracking().AnyAsync(x => x.Username.Value == username, cancellationToken);
    }
}
