using CryptoCheck.Core.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoCheck.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<T> GetItemAsync<T>(string cacheKey, Func<Task<T>> cacheRefreshFunc, CancellationToken cancellationToken = default)
        {
            //check if the item exists in the cache
            var cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (cachedValue != null)
            {
                return JsonConvert.DeserializeObject<T>(cachedValue);
            }

            //otherwise get the new item and store it
            var newItemToCache = await cacheRefreshFunc();

            await SetItemAsync(cacheKey, newItemToCache, cancellationToken);

            return newItemToCache;
        }

        public async Task SetItemAsync<T>(string cacheKey, T item, CancellationToken cancellationToken)
        {
            var cacheItem = JsonConvert.SerializeObject(item);

            await _cache.SetStringAsync(
                cacheKey,
                cacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ICacheService.absoluteExpirationInHours
                },
                cancellationToken
            );
        }
    }
}
