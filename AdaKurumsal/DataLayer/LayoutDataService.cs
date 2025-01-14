using AdaKurumsal.Models.PageModels;

namespace AdaKurumsal.DataLayer
{
    public interface ILayoutDataService
    {
        Task<LayoutModel> GetLayout();
    }

    public class LayoutDataService : ILayoutDataService
    {
        private readonly CacheManager _cacheManager;
        private readonly IIletisimDataService _iletisimDataService;

        private const string LAYOUT_CACHE_KEY_PREFIX = Tools.CachePrefixes.LAYOUT_CACHE_KEY_PREFIX;
        public LayoutDataService(IIletisimDataService iletisimDataService, CacheManager cacheManager)
        {
            _iletisimDataService = iletisimDataService;
            _cacheManager = cacheManager;
        }
        public async Task<LayoutModel> GetLayout()
        {
            var currentLanguage = Tools.Kit.GetLanguage();


            string cacheKey = $"{LAYOUT_CACHE_KEY_PREFIX}_{currentLanguage}";

            return await _cacheManager.GetOrCreateAsync(cacheKey, async () =>
            {
                var layout = new LayoutModel
                {
                    Iletisim = await _iletisimDataService.GetIletisim(currentLanguage)
                };

                return layout;
            });
        }
    }
}
