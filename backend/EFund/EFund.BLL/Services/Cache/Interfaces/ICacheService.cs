using EFund.Common.Enums;
using LanguageExt;

namespace EFund.BLL.Services.Cache.Interfaces;

public interface ICacheService
{
    Task<Option<T>> GetAsync<T>(CachingKey key);
    Task<Option<T>> GetAsync<T>(CachingKey key, object keyParameter);
    Task<Option<T>> GetAsync<T>(CachingKey key, IEnumerable<object> keyParameters);

    Task SetAsync<T>(CachingKey key, T value,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task SetAsync<T>(CachingKey key, object keyParameter, T value,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task SetAsync<T>(CachingKey key, IEnumerable<object> keyParameters, T value,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task<T> GetOrSetAsync<T>(CachingKey key, Func<T> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task<T> GetOrSetAsync<T>(CachingKey key, object keyParameter, Func<T> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task<T> GetOrSetAsync<T>(CachingKey key, IEnumerable<object> keyParameters, Func<T> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task<T> GetOrSetAsync<T>(CachingKey key, Func<Task<T>> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task<T> GetOrSetAsync<T>(CachingKey key, object keyParameter, Func<Task<T>> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task<T> GetOrSetAsync<T>(CachingKey key, IEnumerable<object> keyParameters, Func<Task<T>> valueFactory,
        TimeSpan? slidingLifetime = null, TimeSpan? absoluteLifetime = null);

    Task RemoveAsync(CachingKey key, object keyParameter);
    Task RemoveAsync(CachingKey key, IEnumerable<object> keyParameters);
}