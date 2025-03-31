using Ecommerce.SharedKernel.Response;

namespace Ecommerce.Domain.Contracts;

public interface IRestCountryService
{
    Task<RestCountryResponse[]> GetCountriesInfoAsync(CancellationToken cancellationToken = default);
}
