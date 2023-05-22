namespace EventService.Domain.Interfaces
{
    public interface IRedisService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object data, int cacheTimeInMinutes);
        Task RemoveAsync(string key);

    }
}
