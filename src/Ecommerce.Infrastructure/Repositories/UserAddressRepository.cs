using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class UserAddressRepository
    : GenericRepository<UserAddress>, IUserAddressRepository
{
    public UserAddressRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<bool> isDefaultAddressSet(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.UserAddress.AsNoTracking().AnyAsync(x => x.IsDefault && x.User.Username.Value == username);
    }
}
