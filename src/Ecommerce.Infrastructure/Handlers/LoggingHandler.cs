using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Handlers;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try 
        {
            _logger.LogInformation("Sending request: {method} {url}", request.Method, request.RequestUri);

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken); 

            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Received response: {statusCode} for {url}", response.StatusCode, request.RequestUri);

            return response;

        } catch (Exception ex)
        {
            _logger.LogError("An error occurred while sending request to {url} with the error message: {errorMessage}",
                             request.RequestUri,
                             ex.Message);
            throw;
        }
    }
}
