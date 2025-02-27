using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ecommerce.SharedKernel.Contracts;

namespace Ecommerce.Infrastructure.Persistence.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateSoftDeleteEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateSoftDeleteEntities(DbContext dbContext)
    {
        IEnumerable<EntityEntry<ISoftDeletable>> entries = dbContext.ChangeTracker
                                                                    .Entries<ISoftDeletable>()
                                                                    .Where(x => x.State == EntityState.Deleted);
        if (!entries.Any())
        {
            return;
        }

        foreach (EntityEntry<ISoftDeletable> entry in entries)
        {
            // Changing state of the delete entries to modified
            entry.State = EntityState.Modified;

            // Updating some important properties of the entity
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAtUtc = DateTime.UtcNow;
        }
    }
}
