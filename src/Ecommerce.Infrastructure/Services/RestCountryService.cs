using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Response;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Ecommerce.Infrastructure.Services;

public class RestCountryService : IRestCountryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RestCountryService> _logger;

    public RestCountryService(
        HttpClient httpClient,
        ILogger<RestCountryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<RestCountryResponse[]> GetCountriesInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using HttpResponseMessage request = await _httpClient.GetAsync("/v3.1/all?fields=name");

            request.EnsureSuccessStatusCode();

            RestCountryResponse[]? response = await request.Content.ReadFromJsonAsync<RestCountryResponse[]>(cancellationToken);

            if (response is null || response.Length <= 0)
            {
                throw new InvalidOperationException("The response from RestCountry API was null.");
            }

            return response;

        } catch (Exception ex)
        {
            _logger.LogError(
                 "The request was not completed successfully. Error message: {ErrorMessage}, Exception: {Exception}",
                 ex.Message,
                 ex);

            throw;
        }
    }
}
