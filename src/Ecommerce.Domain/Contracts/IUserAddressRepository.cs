using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IUserAddressRepository 
    : IRepositoryWriter<UserAddress>
{
    Task<bool> isDefaultAddressSet(string username,CancellationToken cancellationToken = default);
}
