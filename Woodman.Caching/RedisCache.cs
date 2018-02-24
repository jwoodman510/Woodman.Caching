using System;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Woodman.Caching
{
    public interface IRedisCache : ICache
    {

    }

    public class RedisCache : IRedisCache
    {
        private IDistributedCache _cache;

        public RedisCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.GetAsync(key);

            if (value == null)
                return default(T);

            return MessagePackSerializer.Deserialize<T>(value);
        }

        public async Task<object> GetAsync(string key)
        {
            var value = await _cache.GetAsync(key);

            if (value == null)
                return null;

            return MessagePackSerializer.Deserialize<object>(value);
        }

        public async Task SetAsync(string key, object value, DateTimeOffset absoluteExpiration)
        {
            await _cache.SetAsync(key, MessagePackSerializer.Serialize(value), new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteExpiration });
        }

        public async Task SetAsync(string key, object value, TimeSpan absoluteExpirationRelativeToNow)
        {
            await _cache.SetAsync(key, MessagePackSerializer.Serialize(value), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow });
        }

        public async Task SetSlidingAsync(string key, object value, TimeSpan slidingExpiration)
        {
            await _cache.SetAsync(key, MessagePackSerializer.Serialize(value), new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration });
        }

        public async Task DeleteAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public void Dispose()
        {
            _cache = null;
        }
    }
}
