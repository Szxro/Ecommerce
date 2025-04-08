using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class ProductCategoryRepository 
    : GenericRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<ProductCategory?> GetProductCategoryByNameAsync(string categoryName, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.ProductCategory
                                  .Where(x => x.Name.ToLower() == categoryName.ToLower()).FirstOrDefaultAsync(cancellationToken);
    }
}
