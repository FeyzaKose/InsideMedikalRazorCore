using AdaKurumsal.DataLayer;
using AdaKurumsal.Models;
using AdaKurumsal.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AdaKurumsal.Pages.Admin.IletisimInfo
{
    public class IletisimModel : PageModel
    {
        [BindProperty]
        public Iletisim iletisimTR { get; set; }

        [BindProperty]
        public Iletisim iletisimEN { get; set; }

        [BindProperty]
        public string dil { get; set; }

        private readonly EFContext _context;
        private readonly IIletisimDataService _iletisimDataService;
        private readonly CacheManager _cacheManager;

        public IletisimModel(EFContext context, IIletisimDataService iletisimDataService, CacheManager cacheManager)
        {
            _context = context;
            _iletisimDataService = iletisimDataService;
            _cacheManager = cacheManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            iletisimTR = await _iletisimDataService.GetIletisim("tr");
            iletisimEN = await _iletisimDataService.GetIletisim("en");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AdaResponse resp = new AdaResponse();
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();
                resp.Sonuc = false;
                resp.Mesaj = JsonConvert.SerializeObject(errors);
                return Content(JsonConvert.SerializeObject(resp));
            }
            if (dil == "tr")
            {
                await _iletisimDataService.UpdateIletisim(iletisimTR);
                await _cacheManager.RefreshCacheAsync(Tools.CachePrefixes.LAYOUT_CACHE_KEY_PREFIX + "tr");


            }
            else
            {
                await _iletisimDataService.UpdateIletisim(iletisimEN);
                await _cacheManager.RefreshCacheAsync(Tools.CachePrefixes.LAYOUT_CACHE_KEY_PREFIX + "en");
            }
            resp.Sonuc = true;
            return Content(JsonConvert.SerializeObject(resp));
        }
    }
}
