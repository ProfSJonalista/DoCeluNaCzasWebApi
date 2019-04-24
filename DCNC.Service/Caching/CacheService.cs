using System;
using System.Runtime.Caching;

namespace DCNC.Service.Caching
{
    public static class CacheService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public static void CacheData<T>(T data, string cacheKey)
        {
            _cache.Set(cacheKey, data, new CacheItemPolicy());
        }

        public static T GetData<T>(string cacheKey) where T : class
        {
            return _cache[cacheKey] as T;
        }

        public static DateTime GetData(string cacheKey)
        {
            return _cache[cacheKey] is DateTime ? (DateTime)_cache[cacheKey] : new DateTime();
        }
    }
}