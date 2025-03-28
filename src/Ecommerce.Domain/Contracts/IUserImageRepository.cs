using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IUserImageRepository 
    : IRepositoryWriter<UserImage>
{
    Task<UserImage?> GetActiveUserImageByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
