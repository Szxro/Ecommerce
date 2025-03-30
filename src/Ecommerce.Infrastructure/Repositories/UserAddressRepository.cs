using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class UserAddressRepository
    : GenericRepository<UserAddress>, IUserAddressRepository
{
    public UserAddressRepository(AppDbContext appDbContext) : base(appDbContext) { }
}
