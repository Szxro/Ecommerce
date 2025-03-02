using Ecommerce.Domain.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Infrastructure.Attributes;

namespace Ecommerce.Infrastructure.Services;

[Inject(ServiceLifetime.Singleton)] // Memory Cache is designed to be use as singleton
public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(
        IMemoryCache memoryCache,
        ILogger<CacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true,
    };

    private static readonly MemoryCacheEntryOptions DefaultOptions = new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        Priority = CacheItemPriority.Normal,
    };

    public void Set<T>(string key, T value, MemoryCacheEntryOptions? options = null)
    {
        try
        {
            string serializedValue = JsonSerializer.Serialize(value, SerializerOptions);

            _memoryCache.Set(key, serializedValue, options ?? DefaultOptions);

        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An unexpected error happen trying to set a value to the cache with the error message '{message}'.",
                ex.Message);

            throw;
        }
    }
    public T? Get<T>(string key)
    {
        try
        {
            string? value = _memoryCache.Get<string>(key);

            if (value is null)
            {
                return default;
            }

            T? deserializedValue = JsonSerializer.Deserialize<T>(value);

            return deserializedValue;

        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An unexpected error happen trying to retrieve a value from the cache with the error message '{message}'.",
                ex.Message);

            throw;
        }
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}