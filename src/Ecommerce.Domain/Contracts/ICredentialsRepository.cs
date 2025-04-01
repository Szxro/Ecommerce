using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface ICredentialsRepository 
    : IRepositoryWriter<Credentials>
{
    Task<Credentials?> GetCredentialsByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
