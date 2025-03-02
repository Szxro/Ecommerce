using Ecommerce.SharedKernel.Contracts;

namespace Ecommerce.Domain.Contracts;

public interface IRepositoryReader<TEntity>
    where TEntity : IEntity
{
    Task<TEntity?> GetById(int id);
}
