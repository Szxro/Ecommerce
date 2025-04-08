using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IProductRepository 
    : IRepositoryWriter<Product>, IRepositoryRemover<Product>
{
    Task<bool> IsProductNameNotUnique(string productName, CancellationToken cancellationToken = default);

    Task<Product?> GetProductByNameAsync(string productName, CancellationToken cancellationToken = default);
}
