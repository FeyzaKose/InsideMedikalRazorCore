using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace AdaKurumsal.DataLayer
{
    public interface ICacheManager
    {
        Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? expiration = null);
        void Remove(string cacheKey);
        Task NotifyCacheUpdatedAsync(string key, string language, string modelName);
        Task RefreshCacheAsync(string modelName, string language = null);
    }
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHubContext<AdaKurumsalHub> _hubContext;

        public CacheManager(IMemoryCache memoryCache, IHubContext<AdaKurumsalHub> hubContext)
        {
            _memoryCache = memoryCache;
            _hubContext = hubContext;
        }

        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(cacheKey, out T cacheEntry))
            {
                return cacheEntry;
            }

            var data = await factory();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = expiration ?? TimeSpan.FromDays(90)
            };
            _memoryCache.Set(cacheKey, data, cacheOptions);

            return data;
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public async Task NotifyCacheUpdatedAsync(string key, string language, string modelName)
        {
            await _hubContext.Clients.Group(language).SendAsync("CacheUpdated", new
            {
                Key = key,
                Language = language,
                ModelName = modelName,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "Admin"
            });
        }

        public async Task RefreshCacheAsync(string cacheKeyPrefix, string language = null)
        {

            if (language != null)
            {
                string cacheKey = $"{cacheKeyPrefix}_{language}";
                Remove(cacheKey);
                await NotifyCacheUpdatedAsync(cacheKeyPrefix, language, cacheKeyPrefix);
            }
            else
            {
                foreach (var lang in Tools.Kit.GetSupportedCultures())
                {
                    string cacheKey = $"{cacheKeyPrefix}_{lang}";
                    Remove(cacheKey);
                }
                await NotifyCacheUpdatedAsync(cacheKeyPrefix, "all", cacheKeyPrefix);
            }
        }
    }
}
