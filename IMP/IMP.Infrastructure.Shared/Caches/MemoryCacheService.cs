using IMP.Application.Interfaces;
using IMP.Domain.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Shared.Caches
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheSettings _cacheSettings;
        private MemoryCacheEntryOptions _cacheOptions;

        public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
        {
            _memoryCache = memoryCache;
            _cacheSettings = cacheSettings.Value;
            if (_cacheSettings != null)
            {
                _cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(_cacheSettings.AbsoluteExpirationInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes),
                    Priority = CacheItemPriority.High
                };
            }
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public T Set<T>(string cacheKey, T value)
        {
            return _memoryCache.Set(cacheKey, value, _cacheOptions);
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            _memoryCache.TryGetValue(cacheKey, out value);
            return value != null;
        }
    }
}
