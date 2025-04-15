using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IShoppingCartRepository 
    : IRepositoryWriter<ShoppingCart>
{
    Task<ShoppingCart?> GetActiveShoppingCartsByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
