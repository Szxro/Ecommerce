using Ecommerce.Infrastructure.Persistence;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.Common;

public abstract class GenericRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly AppDbContext _appDbContext;

    protected GenericRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add(TEntity entity)
    {
        _appDbContext.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _appDbContext.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _appDbContext.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _appDbContext.Set<TEntity>().RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        _appDbContext.Set<TEntity>().Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _appDbContext.Set<TEntity>().UpdateRange(entities);
    }

    public async Task<TEntity?> GetById(int id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int?> RemoveById(int id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Set<TEntity>().Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<int?> BulkDelete(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Set<TEntity>().Where(filter).ExecuteDeleteAsync(cancellationToken);
    }
}
