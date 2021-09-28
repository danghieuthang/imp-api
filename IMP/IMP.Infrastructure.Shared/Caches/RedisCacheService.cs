using IMP.Application.Interfaces;
using IMP.Domain.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Shared.Caches
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly CacheSettings _cacheSettings;
        private readonly DistributedCacheEntryOptions _options;

        public RedisCacheService(IDistributedCache distributedCache, IOptions<CacheSettings> cacheSettings)
        {
            _distributedCache = distributedCache;
            _cacheSettings = cacheSettings.Value;
            if (_cacheSettings != null)
            {
                _options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(_cacheSettings.AbsoluteExpirationInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes)
                };
            }
        }

        public void Remove(string cacheKey)
        {
            _distributedCache.Remove(cacheKey);
        }

        public T Set<T>(string cacheKey, T value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            var redisValue = Encoding.UTF8.GetBytes(serializedValue);
            _distributedCache.Set(cacheKey, redisValue, _options);
            return value;
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            var redisValue = _distributedCache.Get(cacheKey);
            if (redisValue != null)
            {
                var serializedValue = Encoding.UTF8.GetString(redisValue);
                value = JsonConvert.DeserializeObject<T>(serializedValue);
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
