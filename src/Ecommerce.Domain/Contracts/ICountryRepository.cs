using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface ICountryRepository 
    : IRepositoryWriter<Country>
{
    Task<Country?> GetCountryByCountryNameAsync(string countryName, CancellationToken cancellationToken = default);
}
