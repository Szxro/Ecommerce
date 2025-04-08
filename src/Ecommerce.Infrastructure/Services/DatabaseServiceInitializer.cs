using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Domain.Entities;
using Ecommerce.SharedKernel.Response;

namespace Ecommerce.Infrastructure.Services;

[Inject(ServiceLifetime.Scoped)]
public sealed class DatabaseServiceInitializer : IDatabaseServiceInitializer
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DatabaseServiceInitializer> _logger;
    private readonly RestCountryService _restCountryService;
    private readonly IExponentialBackoffService _exponentialBackoffService;

    public DatabaseServiceInitializer(
        AppDbContext appDbContext,
        ILogger<DatabaseServiceInitializer> logger,
        RestCountryService restCountryService,
        IExponentialBackoffService exponentialBackoffService)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _restCountryService = restCountryService;
        _exponentialBackoffService = exponentialBackoffService;
    }

    public async Task CanConnectAsync(CancellationToken cancellationToken = default)
    {
        bool isConnected = await _appDbContext.Database.CanConnectAsync(cancellationToken);

        if (!isConnected)
        {
            _logger.LogWarning("Cant connect to the database, database not available");

            throw new InvalidOperationException();
        }

        _logger.LogInformation("Successfully connect to the database!!!.");
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _appDbContext.Database.MigrateAsync(cancellationToken);

            _logger.LogInformation("Successfully apply the migrations!!.");

        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An error happen trying to apply migrations to the database {providerName} with the error message: {message}",
                _appDbContext.Database.ProviderName,
                ex.Message);

            throw;
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to seed data....");

            if (!await _appDbContext.Set<Country>().AnyAsync(cancellationToken))
            {
                RestCountryResponse[] response = await _restCountryService.GetCountriesInfoAsync(cancellationToken);

                if (response.Length <= 0) return;

                List<Country> countries = response.Select(x => new Country { Name = x.Name.Common }).ToList();

                _appDbContext.Set<Country>().AddRange(countries);

                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            if (!await _appDbContext.Set<ProductCategory>().AnyAsync())
            {
                ProductCategory[] categories =                  
                [
                    new ProductCategory { Name = "Electronics" },
                    new ProductCategory { Name = "Clothing and Accessories" },
                    new ProductCategory { Name = "Food" },
                    new ProductCategory { Name = "Home and Garden" },
                    new ProductCategory { Name = "Beauty and Personal Care" },
                    new ProductCategory { Name = "Toys and Games" },
                    new ProductCategory { Name = "Sports and Outdoors" },
                    new ProductCategory { Name = "Health and Wellness" },
                    new ProductCategory { Name = "Automotive" },
                    new ProductCategory { Name = "Office Supplies" },
                    new ProductCategory { Name = "Books and Music" },
                    new ProductCategory { Name = "Furniture" },
                    new ProductCategory { Name = "Tools and Home Improvement" },
                    new ProductCategory { Name = "Arts and Crafts" }
                ];

                _appDbContext.ProductCategory.AddRange(categories);

                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Seed Successfully!!!");

        } catch (Exception ex)
        {
            _logger.LogError(
                "An error happen trying to seed data to some entities with the error message: {errorMessage}",
                ex.Message);

            throw;
        }
    }
}
