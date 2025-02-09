using AutoMapper;
using Identity.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SitoDeiSiti.DTOs.ConfigSettings;

namespace Identity.Services
{
    public class CacheManager : ICache
    {
        public readonly IMemoryCache MemoryCache;
        public MemoryCacheEntryOptions CacheOptions { get; }

        public CacheManager(IOptions<Cache> options, IMemoryCache _memoryCache)
        {
            MemoryCache = _memoryCache;
            CacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(options.Value.CacheExpirationMinutes));
        }

        public Task<T> GetAsync<T>(string key)
        {
            T result;

            try
            {
                MemoryCache.TryGetValue(key, out T value);

                if (value != null)
                {
                    result = value;
                    return Task.FromResult(result);
                }
                else
                {
                    return Task.FromResult(default(T));
                }
            }
            catch(Exception ex)
            {
                return Task.FromResult(default(T));
            }

        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAsync<T>(string key, T value, TimeSpan? exp = null)
        {
            try
            {
                if (exp.HasValue)
                {
                    MemoryCache.Set(key, value, exp.Value);
                }
                else
                {
                    MemoryCache.Set(key, value);
                }

                return Task.FromResult(true);
            }
            catch(Exception ex)
            {
                return Task.FromResult(false);
            }
        }
    }
}
