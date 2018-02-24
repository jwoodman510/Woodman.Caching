using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Woodman.Caching
{
    public interface IInMemoryCache : ICache
    {

    }

    public class InMemoryCache : IInMemoryCache
    {
        private readonly IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.Run(() =>
            {
                object cachedValue;

                if (_cache.TryGetValue(key, out cachedValue))
                {
                    return (T)cachedValue;
                }

                return default(T);
            });
        }

        public async Task<object> GetAsync(string key)
        {
            return await Task.Run(() =>
            {
                object cachedValue;

                if (_cache.TryGetValue(key, out cachedValue))
                {
                    return cachedValue;
                }

                return null;
            });
        }

        public async Task SetAsync(string key, object value, DateTimeOffset absoluteExpiration)
        {
            await Task.Run(() => _cache.Set(key, value, absoluteExpiration));
        }

        public async Task SetAsync(string key, object value, TimeSpan absoluteExpirationRelativeToNow)
        {
            await Task.Run(() => _cache.Set(key, value, absoluteExpirationRelativeToNow));
        }

        public async Task SetSlidingAsync(string key, object value, TimeSpan slidingExpiration)
        {
            await Task.Run(() => _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration }));
        }

        public async Task DeleteAsync(string key)
        {
            await Task.Run(() => _cache.Remove(key));
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
