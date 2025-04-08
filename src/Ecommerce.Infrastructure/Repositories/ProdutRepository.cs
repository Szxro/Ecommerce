using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class ProdutRepository : GenericRepository<Product>, IProductRepository
{
    public ProdutRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<bool> IsProductNameNotUnique(string productName, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Product
                                  .AsNoTracking()
                                  .Where(x => x.Name == productName).AnyAsync(cancellationToken);
    }
}
