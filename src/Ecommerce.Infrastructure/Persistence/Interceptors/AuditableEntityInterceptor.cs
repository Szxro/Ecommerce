using Ecommerce.SharedKernel.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Infrastructure.Attributes;

namespace Ecommerce.Infrastructure.Persistence.Interceptors;

[Inject(ServiceLifetime.Singleton)]
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavedChangesAsync(eventData, result, cancellationToken);
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
                SetCurrentDatetimeProperty(entity, nameof(AuditableEntity.CreatedAtUtc), DateTimeOffset.Now);
                SetCurrentDatetimeProperty(entity, nameof(AuditableEntity.ModifiedAtUtc), DateTimeOffset.Now);
            }

            if (entity.State == EntityState.Modified)
            {
                SetCurrentDatetimeProperty(entity, nameof(AuditableEntity.ModifiedAtUtc), DateTimeOffset.Now);
            }
        }
    }

    private void SetCurrentDatetimeProperty(
       EntityEntry entity,
       string propertyName,
       DateTimeOffset dateTime) => entity.Property(propertyName).CurrentValue = dateTime;
}
