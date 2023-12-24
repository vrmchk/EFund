using EFund.Common.Models.Configs;
using LanguageExt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services.Cache;

public class MemoryCacheService : CacheServiceBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MemoryCacheService> _logger;

    public MemoryCacheService(CacheConfig cacheConfig, IMemoryCache memoryCache, ILogger<MemoryCacheService> logger)
        : base(cacheConfig)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    protected override Task<Option<T>> GetAsync<T>(string cachingKey)
    {
        _logger.LogInformation("Cache GetAsync: {CachingKey}", cachingKey);
        var result = _memoryCache.Get<T>(cachingKey);
        return Task.FromResult<Option<T>>(result != null ? result : None);
    }

    protected override Task SetAsync<T>(string cachingKey, T value, TimeSpan? slidingLifetime = null,
        TimeSpan? absoluteLifetime = null)
    {
        _logger.LogInformation("Cache SetAsync: {CachingKey}", cachingKey);
        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(slidingLifetime ?? _cacheConfig.SlidingLifetime)
            .SetAbsoluteExpiration(absoluteLifetime ?? _cacheConfig.AbsoluteLifetime);

        _memoryCache.Set(cachingKey, value, options);
        return Task.CompletedTask;
    }

    protected override Task RemoveAsync(string cachingKey)
    {
        _logger.LogInformation("Cache RemoveAsync: {CachingKey}", cachingKey);
        _memoryCache.Remove(cachingKey);
        return Task.CompletedTask;
    }
}