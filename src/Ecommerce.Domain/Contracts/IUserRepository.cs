using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IUserRepository 
    : IRepositoryWriter<User>
{
    Task<bool> IsUsernameNotUniqueAsync(string username,CancellationToken cancellationToken = default);

    Task<bool> IsEmailNotUniqueAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
