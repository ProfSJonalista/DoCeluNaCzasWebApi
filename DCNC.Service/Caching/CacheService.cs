using System;
using System.Runtime.Caching;

namespace DCNC.Service.Caching
{
    public static class CacheService
    {
        static readonly ObjectCache Cache = MemoryCache.Default;

        public static void CacheData<T>(T data, string cacheKey)
        {
            Cache.Set(cacheKey, data, new CacheItemPolicy());
        }

        public static T GetData<T>(string cacheKey) where T : class
        {
            return Cache[cacheKey] as T;
        }

        public static DateTime GetData(string cacheKey)
        {
            return Cache[cacheKey] is DateTime ? (DateTime)Cache[cacheKey] : new DateTime();
        }
    }
}