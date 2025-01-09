using AdaKurumsal.Models.PageModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AdaKurumsal.DataLayer
{
    public class LayoutHub : Hub
    {
        public async Task JoinLanguageGroup(string language)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, language);
        }

        public async Task LeaveLanguageGroup(string language)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, language);
        }
    }
    public class LayoutDataService : ILayoutDataService
    {
        private readonly EFContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHubContext<LayoutHub> _hubContext;
        private const string CACHE_KEY_PREFIX = "Layout_";
        public LayoutDataService(EFContext context, IMemoryCache memoryCache, IHubContext<LayoutHub> hubContext)
        {
            _context = context;
            _memoryCache = memoryCache;
            _hubContext = hubContext;
        }
        public async Task<LayoutModel> GetLayout()
        {
            var currentLanguage = Tools.Kit.GetLanguage();
            string cacheKey = $"{CACHE_KEY_PREFIX}{currentLanguage}"; // Dile özel cache key

            // Cache'den kontrol et
            if (_memoryCache.TryGetValue<LayoutModel>(cacheKey, out var cachedLayout))
            {
                return cachedLayout;
            }

            // Cache'de yoksa yeni veri oluştur
            var layout = new LayoutModel
            {
                Iletisim = await _context.Iletisim
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Language == currentLanguage)
            };

            // Cache'e kaydet
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(cacheKey, layout, cacheOptions);

            return layout;
        }



        // Cache'i temizle ve diğer kullanıcıları bilgilendir
        public async Task RefreshLayout(string language = null)
        {
            if (language != null)
            {
                // Sadece belirtilen dilin cache'ini temizle
                string cacheKey = $"{CACHE_KEY_PREFIX}{language}";
                _memoryCache.Remove(cacheKey);

                // SignalR ile sadece o dildeki kullanıcılara bildirim gönder
                await _hubContext.Clients.Group(language).SendAsync("LayoutUpdated", new
                {
                    Language = language,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = "Admin"
                });
            }
            else
            {
                // Tüm dillerin cache'ini temizle
                foreach (var lang in new[] { "tr", "en" })
                {
                    string cacheKey = $"{CACHE_KEY_PREFIX}{lang}";
                    _memoryCache.Remove(cacheKey);
                }

                // Tüm kullanıcılara bildirim gönder
                await _hubContext.Clients.All.SendAsync("LayoutUpdated", new
                {
                    Language = "all",
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = "Admin"
                });
            }
        }
    }
}
