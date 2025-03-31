using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.SharedKernel.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Services;

// TODO: Refactor the logic (one method for both cases) and test it.

[Inject(ServiceLifetime.Singleton)]
public class ExponentialBackoffService : IExponentialBackoffService
{
    private readonly Random _random = new Random();
    private readonly ILogger<ExponentialBackoffService> _logger;

    public ExponentialBackoffService(ILogger<ExponentialBackoffService> logger)
    {
        _logger = logger;
    }

    public async Task<T?> RetryWithBackoff<T>(Func<Task<T>> func, BackOffOptions? options = null, CancellationToken cancellationToken = default)
    {
        (int maxRetries, int initialDelay, int maxDelay, int timeMultiple) = options ?? new BackOffOptions();

        int attempts = 0;

        while (attempts < maxRetries)
        {
            try
            {
                T result = await func();

                _logger.LogInformation(
                    "Operation succeeded after retry count {attempts}/{maxRetries} retries.",
                    attempts,
                    maxRetries);

                return result;

            } catch
            {
                double waitTime = Math.Min(initialDelay * Math.Pow(timeMultiple, attempts), maxDelay);

                int delay = (int)Math.Floor(_random.NextDouble() * waitTime);

                await Task.Delay(delay, cancellationToken);

                attempts++;

                _logger.LogError(
                    "Operation failed after retry count {attempts}/{maxRetries} and {delay}ms.",                    
                    attempts,
                    maxRetries,
                    delay);
            }
        }

        _logger.LogError("Max attempts reached operation failed!!!!.");

        return default(T);
    }

    public async Task RetryWithBackoff(Func<Task> func, BackOffOptions? options = null, CancellationToken cancellationToken = default)
    {
        (int maxRetries, int initialDelay, int maxDelay, int timeMultiple) = options ?? new BackOffOptions();

        int attempts = 0;

        while (attempts < maxRetries)
        {
            try
            {
                await func();

                _logger.LogInformation(
                    "Operation succeeded after retry count {attempts}/{maxRetries} retries.",
                    attempts,
                    maxRetries);

                return;

            }
            catch
            {
                double waitTime = Math.Min(initialDelay * Math.Pow(timeMultiple, attempts), maxDelay);

                int delay = (int)Math.Floor(_random.NextDouble() * waitTime);

                await Task.Delay(delay, cancellationToken);

                attempts++;

                _logger.LogError(
                    "Operation failed after retry count {attempts}/{maxRetries} and {delay}ms.",
                    attempts,
                    maxRetries,
                    delay);
            }
        }

        _logger.LogError("Max attempts reached operation failed!!!!.");
    }
}
