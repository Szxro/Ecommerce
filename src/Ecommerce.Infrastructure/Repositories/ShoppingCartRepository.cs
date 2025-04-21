using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class ShoppingCartRepository
    : GenericRepository<ShoppingCart>, IShoppingCartRepository
{
    public ShoppingCartRepository(AppDbContext appDbContext) : base(appDbContext) { }    

    public async Task<ShoppingCart?> GetActiveShoppingCartsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _appDbContext
                        .ShoppingCart
                        .Include(x => x.ShoppingCartDetails)
                        .Where(x => x.User.Username.Value == username && !x.IsSubmit)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}
