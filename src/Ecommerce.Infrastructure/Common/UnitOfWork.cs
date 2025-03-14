﻿using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Extensions.Logging;
using Ecommerce.SharedKernel.Common;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Infrastructure.Channels;

namespace Ecommerce.Infrastructure.Common;

[Inject(ServiceLifetime.Scoped)]
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<AppDbContext> _logger;
    private readonly DomainEventChannel _eventChannel;

    public UnitOfWork(
        AppDbContext appDbContext,
        ILogger<AppDbContext> logger,
        DomainEventChannel eventChannel)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _eventChannel = eventChannel;
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public void ChangeTrackerToUnchanged<T>(T entity) where T : class
    {
        _appDbContext.Set<T>().Entry(entity).State = EntityState.Unchanged;
    }

    public async Task<int?> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _appDbContext.SaveChangesAsync(cancellationToken);

            List<IDomainEvent> events = _appDbContext.ChangeTracker.Entries<Entity>()
                                      .Select(x => x.Entity)
                                      .Where(x => x.DomainEvent.Any())
                                      .SelectMany(entity => entity.DomainEvent)
                                      .ToList();

            if (!events.Any()) return result;

            foreach (IDomainEvent @event in events)
            {
                await _eventChannel.AddEventAsync(@event, cancellationToken);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An exception happen trying to save changes into the database {providerName} with the error message : {message}",
                _appDbContext.Database.ProviderName,
                ex.Message);

            throw;
        }
    }
}
