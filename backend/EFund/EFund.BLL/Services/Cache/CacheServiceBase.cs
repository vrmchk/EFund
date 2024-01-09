using EFund.BLL.Services.Cache.Interfaces;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using LanguageExt;

namespace EFund.BLL.Services.Cache;

public abstract class CacheServiceBase : ICacheService
{
    protected readonly CacheConfig _cacheConfig;

    protected CacheServiceBase(CacheConfig cacheConfig)
    {
        _cacheConfig = cacheConfig;
    }

    protected abstract Task<Option<T>> GetAsync<T>(string cachingKey);

    protected abstract Task SetAsync<T>(string cachingKey, T value,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    protected abstract Task RemoveAsync(string cachingKey);

    public Task<Option<T>> GetAsync<T>(CachingKey key, object keyParameter)
    {
        return GetAsync<T>(key, new[] { keyParameter });
    }

    public Task<Option<T>> GetAsync<T>(CachingKey key, IEnumerable<object> keyParameters)
    {
        var cachingKey = GetCachingKey(key, keyParameters);
        return GetAsync<T>(cachingKey);
    }

    public Task SetAsync<T>(CachingKey key, object keyParameter, T value, TimeSpan? slidingLifetime = null,
        TimeSpan? absoluteLifetime = null)
    {
        return SetAsync(key, new[] { keyParameter }, value, slidingLifetime, absoluteLifetime);
    }

    public Task SetAsync<T>(CachingKey key, IEnumerable<object> keyParameters, T value,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null)
    {
        var cachingKey = GetCachingKey(key, keyParameters);
        return SetAsync(cachingKey, value, slidingLifetime, absoluteLifetime);
    }

    public Task<T> GetOrSetAsync<T>(CachingKey key, object keyParameter, Func<T> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null)
    {
        return GetOrSetAsync(key, new[] { keyParameter }, valueFactory, slidingLifetime, absoluteLifetime);
    }

    public async Task<T> GetOrSetAsync<T>(CachingKey key, IEnumerable<object> keyParameters, Func<T> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null)
    {
        var cachingKey = GetCachingKey(key, keyParameters);
        var result = await GetAsync<T>(cachingKey);
        return await result.Match<Task<T>>(
            Some: Task.FromResult,
            None: async () =>
            {
                var value = valueFactory();
                await SetAsync(cachingKey, value, slidingLifetime, absoluteLifetime);
                return value;
            });
    }

    public Task<T> GetOrSetAsync<T>(CachingKey key, object keyParameter, Func<Task<T>> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null)
    {
        return GetOrSetAsync(key, new[] { keyParameter }, valueFactory, slidingLifetime, absoluteLifetime);
    }

    public async Task<T> GetOrSetAsync<T>(CachingKey key, IEnumerable<object> keyParameters, Func<Task<T>> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null)
    {
        var cachingKey = GetCachingKey(key, keyParameters);
        var result = await GetAsync<T>(cachingKey);
        return await result.Match<Task<T>>(
            Some: Task.FromResult,
            None: async () =>
            {
                var value = await valueFactory();
                await SetAsync(cachingKey, value, slidingLifetime, absoluteLifetime);
                return value;
            });
    }

    public Task RemoveAsync(CachingKey key, object keyParameter)
    {
        return RemoveAsync(key, new[] { keyParameter });
    }

    public Task RemoveAsync(CachingKey key, IEnumerable<object> keyParameters)
    {
        var cachingKey = GetCachingKey(key, keyParameters);
        return RemoveAsync(cachingKey);
    }

    private string GetCachingKey(CachingKey key, IEnumerable<object> keyParameters)
    {
        var parameterString = string.Join("_", keyParameters);
        return $"{key}_{parameterString}";
    }
}