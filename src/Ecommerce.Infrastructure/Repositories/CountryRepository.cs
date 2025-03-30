using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class CountryRepository
    : GenericRepository<Country>, ICountryRepository
{
    public CountryRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Country?> GetCountryByCountryNameAsync(string countryName, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Country.Where(x => x.Name == countryName).FirstOrDefaultAsync(cancellationToken);
    }
}
