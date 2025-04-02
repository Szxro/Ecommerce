using Ecommerce.Domain.Contracts;

namespace Ecommerce.Infrastructure.Handlers;

public class RetryHandler : DelegatingHandler
{
    private readonly IExponentialBackoffService _exponentialBackoffService;

    public RetryHandler(IExponentialBackoffService exponentialBackoffService)
    {
        _exponentialBackoffService = exponentialBackoffService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = await _exponentialBackoffService.RetryWithBackoff(
            () => base.SendAsync(request, cancellationToken),
            cancellationToken: cancellationToken);

        return response ?? throw new HttpRequestException("Request failed after all retries.");
    }
}
