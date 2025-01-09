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
        private readonly ILayoutDataService _layoutService;

        public IletisimModel(EFContext context, ILayoutDataService layoutService)
        {
            _context = context;
            _layoutService = layoutService;
        }
        public void OnGet()
        {
            iletisimTR = _context.Iletisim.FirstOrDefault(x => x.Language == "tr");
            iletisimEN = _context.Iletisim.FirstOrDefault(x => x.Language == "en");
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
                _context.Iletisim.Update(iletisimTR);
                _context.SaveChanges();

            }
            else
            {
                _context.Iletisim.Update(iletisimEN);
                _context.SaveChanges();

            }
            await _layoutService.RefreshLayout(dil);
            resp.Sonuc = true;
            return Content(JsonConvert.SerializeObject(resp));
        }
    }
}
