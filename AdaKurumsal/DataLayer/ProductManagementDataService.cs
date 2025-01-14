using AdaKurumsal.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AdaKurumsal.DataLayer
{

    public interface IProductManagementDataService
    {
        Task<List<Category>> GetAllTopLevelCategoriesWithSubCategoriesAsync(string language);

        ////Cache Refresh Metods
        Task RefreshCacheTopLevelCategoriesWithSubCategories(string language);
    }
    public class ProductManagementDataService : IProductManagementDataService
    {
        private readonly EFContext _context;
        private readonly ICacheManager _cacheManager;
        private const string CATEGORIESWITHSUBCATEGORIES_CACHE_KEY_PREFIX = "categorieswithsubcategories";

        public ProductManagementDataService(EFContext context, ICacheManager cacheManager)
        {
            _context = context;
            _cacheManager = cacheManager;
        }

        #region GetTopLevelCategoriesWithSubCategoriesFromCache
        public async Task<List<Category>> GetAllTopLevelCategoriesWithSubCategoriesAsync(string language)
        {
            string cacheKey = $"{CATEGORIESWITHSUBCATEGORIES_CACHE_KEY_PREFIX}_{language}";

            return await _cacheManager.GetOrCreateAsync(cacheKey, async () =>
            {
                var topLevelCategories = await _context.Categories
                    .Where(c => c.MainCategoryCode == null && c.Language == language)
                    .ToListAsync();

                foreach (var category in topLevelCategories)
                {
                    await LoadSubCategoriesRecursive(category, language);
                }

                return topLevelCategories;
            });
        }

        private async Task LoadSubCategoriesRecursive(Category category, string language)
        {
            var subCategories = await _context.Categories
                .Where(c => c.MainCategoryCode == category.Code && c.Language == language)
                .ToListAsync();

            category.SubCategories = subCategories;

            foreach (var subCategory in subCategories)
            {
                await LoadSubCategoriesRecursive(subCategory, language);
            }
        }

        public async Task RefreshCacheTopLevelCategoriesWithSubCategories(string language = null)
        {
            await _cacheManager.RefreshCacheAsync(CATEGORIESWITHSUBCATEGORIES_CACHE_KEY_PREFIX, language);
        }



        #endregion


    }
}
