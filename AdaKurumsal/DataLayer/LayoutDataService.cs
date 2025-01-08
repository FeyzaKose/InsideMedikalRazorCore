using AdaKurumsal.Models.PageModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AdaKurumsal.DataLayer
{
    public class LayoutHub : Hub
    {
        private readonly ILayoutDataService _layoutService;

        public LayoutHub(ILayoutDataService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task RefreshLayout()
        {
            await Clients.All.SendAsync("LayoutUpdated");
        }
    }
    public class LayoutDataService : ILayoutDataService
    {
        private readonly EFContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LayoutDataService> _logger;
        private readonly IHubContext<LayoutHub> _hubContext;
        private const string CACHE_KEY = "Layout_";
        public LayoutDataService(EFContext context, IMemoryCache memoryCache, ILogger<LayoutDataService> logger)
        {
            _context = context;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        public async Task<LayoutModel> GetLayout()
        {
            var currentLanguage = Tools.Kit.GetLanguage();

            // Cache'den kontrol et
            if (_memoryCache.TryGetValue<LayoutModel>(CACHE_KEY, out var cachedLayout))
            {
                return cachedLayout;
            }

            // Cache'de yoksa yeni veri oluştur
            var layout = new LayoutModel
            {
                Iletisim = await _context.Iletisim
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Dil == currentLanguage)
            };

            // Cache'e kaydet
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(CACHE_KEY, layout, cacheOptions);

            return layout;
        }

        // Cache'i temizle ve diğer kullanıcıları bilgilendir
        public async Task RefreshLayout()
        {
            _memoryCache.Remove(CACHE_KEY);
            await _hubContext.Clients.All.SendAsync("LayoutUpdated");
        }
    }
}
