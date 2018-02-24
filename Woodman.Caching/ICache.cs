using System;
using System.Threading.Tasks;

namespace Woodman.Caching
{
    public interface ICache : IDisposable
    {
        Task<T> GetAsync<T>(string key);

        Task<object> GetAsync(string key);

        Task SetAsync(string key, object value, DateTimeOffset absoluteExpiration);

        Task SetAsync(string key, object value, TimeSpan absoluteExpirationRelativeToNow);

        Task SetSlidingAsync(string key, object value, TimeSpan slidingExpiration);

        Task DeleteAsync(string key);
    }
}
