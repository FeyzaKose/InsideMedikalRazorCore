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

        EFContext context;

        public IletisimModel(EFContext _context)
        {
            context = _context;
        }
        public void OnGet()
        {
            iletisimTR = context.Iletisim.FirstOrDefault(x => x.Dil == "tr");
            iletisimEN = context.Iletisim.FirstOrDefault(x => x.Dil == "en");
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
                context.Iletisim.Update(iletisimTR);
                context.SaveChanges();
            }
            else
            {
                context.Iletisim.Update(iletisimEN);
                context.SaveChanges();
            }
            resp.Sonuc = true;
            return Content(JsonConvert.SerializeObject(resp));
        }
    }
}
