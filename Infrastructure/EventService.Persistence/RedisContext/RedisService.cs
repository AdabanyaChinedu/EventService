using EventService.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EventService.Persistence.RedisContext
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _redisCache;


        public RedisService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await _redisCache.GetStringAsync(key);
            return result == null ? default(T)! : JsonSerializer.Deserialize<T>(result)!;
        }

        public async Task SetAsync(string key, object data, int cacheTimeInMinutes)
        {
            var json = JsonSerializer.Serialize(data);
            
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTimeInMinutes));

            await _redisCache.SetStringAsync(key, json, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _redisCache.RemoveAsync(key);
        }
    }
}
