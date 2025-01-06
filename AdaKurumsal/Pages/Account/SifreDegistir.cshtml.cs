using AdaKurumsal.DataLayer;
using AdaKurumsal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AdaKurumsal.Pages.Account
{
    [Authorize]
    public class SifreDegistirModel : PageModel
    {
        [BindProperty]
        public string YeniSifre { get; set; }

        [BindProperty]
        public string SifreTekrar { get; set; }

        EFContext context;
        public SifreDegistirModel(EFContext _context)
        {
            context = _context;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (YeniSifre.Trim() != SifreTekrar.Trim())
            {
                var returnValue = new { mesaj = "Sifreniz eslesmedi", durum = false };
                return Content(JsonConvert.SerializeObject(returnValue));
            }
            else
            {
                AppUser user = await context.AppUsers.Where(x => x.Id.ToString() == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync();
                user.Password = Tools.Kit.HashString(YeniSifre);
                context.AppUsers.Update(user);
                await context.SaveChangesAsync();
                var returnValue = new { mesaj = "Sifreniz Degistirildi", durum = true };
                return Content(JsonConvert.SerializeObject(returnValue));
            }

        }
    }
}
