using AdaKurumsal.DataLayer;
using AdaKurumsal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AdaKurumsal.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public GirisBilgileri bilgi { get; set; }

        public string hata { get; set; }

        protected EFContext context;
        public LoginModel(EFContext _context)
        {
            this.context = _context;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(GirisBilgileri bilgi)
        {

            if (!ModelState.IsValid)
            {

                return Page();
            }
            else
            {
                AppUser kullanici = context.AppUsers.Where(x => x.Email == bilgi.Email).FirstOrDefault();
                if (kullanici == null)
                {
                    hata = "Kullanýcý bulunamadý";
                }
                else if (kullanici.isActive == false)
                {
                    hata = "Kullanýcý aktif deðil";
                }
                else if (!Tools.Kit.VerifyString(bilgi.Sifre, kullanici.Password))
                {
                    hata = "Þifre yanlýþ";
                }

                else
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,kullanici.Id.ToString()),
                            new Claim(ClaimTypes.Name,kullanici.UserName),
                            new Claim(ClaimTypes.Email,bilgi.Email)
                        };

                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("MyCookieAuth", principal);
                    return Redirect("/yonetim");
                }
                return Page();
            }

        }
    }

    public class GirisBilgileri
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Gecerli bir email adresi giriniz")]
        [EmailAddress(ErrorMessage = "Gecerli bir email adresi girinizz")]
        [Required(ErrorMessage = "Email zorunludur")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Sifre  zorunludur")]
        public string Sifre { get; set; }
    }
}
