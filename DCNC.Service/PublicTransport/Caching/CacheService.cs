using System;
using System.Runtime.Caching;

namespace DCNC.Service.PublicTransport.Caching
{
    public class CacheService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public void CacheData<T>(T data, string cacheKey)
        {
            _cache.Set(cacheKey, data, new CacheItemPolicy());
        }

        public T GetData<T>(string cacheKey) where T : class
        {
            return _cache[cacheKey] as T;
        }

        public DateTime GetData(string cacheKey)
        {
            return _cache[cacheKey] is DateTime ? (DateTime)_cache[cacheKey] : new DateTime();
        }
    }
}