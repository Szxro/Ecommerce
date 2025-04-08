using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IProductRepository : IRepositoryWriter<Product>
{
    Task<bool> IsProductNameNotUnique(string productName, CancellationToken cancellationToken = default);
}
