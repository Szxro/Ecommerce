using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IProductCategoryRepository : IRepositoryWriter<ProductCategory>
{
    Task<ProductCategory?> GetProductCategoryByNameAsync(string categoryName,CancellationToken cancellationToken = default);

    Task<bool> IsProductCategoryNameNotUnique(string categoryName, CancellationToken cancellationToken = default);
}
