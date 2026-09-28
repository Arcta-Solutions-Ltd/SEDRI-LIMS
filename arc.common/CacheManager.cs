using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace arc.common
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheManager> _logger;

        public CacheManager(IMemoryCache cache, ILogger<CacheManager> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public async ValueTask<T> GetAsync<T>(Func<Task<T>> func, string key)
        {
            if (_cache.TryGetValue(key, out T cacheItem))
            {
                return cacheItem;
            }
            cacheItem = await func();
            _cache.Set(key, cacheItem);
            _logger.LogInformation($"data retrieved from data source and cached with cache key {key}");
            return cacheItem;
        }
    }
}
