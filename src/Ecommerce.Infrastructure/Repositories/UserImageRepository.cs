using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class UserImageRepository
    : GenericRepository<UserImage>, IUserImageRepository
{
    public UserImageRepository(AppDbContext appDbContext) : base(appDbContext){}

    public async Task<UserImage?> GetActiveUserImageByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.UserImage
                                  .Where(x => x.IsActive && x.User.Username.Value == username)
                                  .FirstOrDefaultAsync(cancellationToken);
    }
}
