using AdaKurumsal.Models.DataModels;
using AdaKurumsal.Models.PageModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AdaKurumsal.DataLayer
{
    public class LayoutHub : Hub
    {

    }
    public class LayoutDataService : ILayoutDataService
    {
        private readonly EFContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LayoutDataService> _logger;
        private readonly IHubContext<LayoutHub> _hubContext;
        private const string CACHE_KEY_PREFIX = "Layout_";
        public LayoutDataService(EFContext context, IMemoryCache memoryCache, ILogger<LayoutDataService> logger)
        {
            _context = context;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        public async Task<LayoutModel> GetLayout()
        {
            string dil = Tools.Kit.getLanguage();
            if (string.IsNullOrEmpty(dil))
            {
                throw new ArgumentNullException(nameof(dil));
            }

            try
            {
                string cacheKey = $"{CACHE_KEY_PREFIX}{dil}";

                return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(5);

                    LayoutModel layout = new LayoutModel();
                    layout.Iletisim = await _context.Iletisim.FirstOrDefaultAsync(x => x.Dil == dil) ?? new Iletisim();

                    if (layout.Iletisim == null)
                    {
                        _logger.LogWarning($"İletişim bilgisi bulunamadı. Dil: {dil}");
                    }

                    return layout;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Layout bilgisi alınırken hata oluştu. Dil: {dil}");
                throw;
            }
        }


    }
}
