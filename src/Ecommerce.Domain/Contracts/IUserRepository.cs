using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IUserRepository 
    : IRepositoryWriter<User>
{
    Task<bool> IsUsernameNotUnique(string username,CancellationToken cancellationToken = default);

    Task<bool> IsEmailNotUnique(string email,CancellationToken cancellationToken = default);
}
