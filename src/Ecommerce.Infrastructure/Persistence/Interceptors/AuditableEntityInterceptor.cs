using Ecommerce.SharedKernel.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {

        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext dbContext)
    {
        List<EntityEntry<AuditableEntity>> entities = dbContext.ChangeTracker.Entries<AuditableEntity>()
                                                                             .Where(entity => entity.State == EntityState.Added || entity.State == EntityState.Modified)
                                                                             .ToList();

        foreach (EntityEntry<AuditableEntity> entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                SetCurrentDatetimeProperty(entity, nameof(AuditableEntity.CreatedAtUtc), DateTime.UtcNow);
                SetCurrentDatetimeProperty(entity, nameof(AuditableEntity.ModifiedAtUtc), DateTime.UtcNow);
            }

            if (entity.State == EntityState.Modified)
            {
                SetCurrentDatetimeProperty(entity, nameof(Entity.ModifiedAtUtc), DateTime.UtcNow);
            }
        }
    }

    private void SetCurrentDatetimeProperty(
       EntityEntry entity,
       string propertyName,
       DateTime dateTime) => entity.Property(propertyName).CurrentValue = dateTime;
}
