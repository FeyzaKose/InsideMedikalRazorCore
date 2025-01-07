using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace AdaKurumsal.Pages.Site
{
    public class HakkimizdaModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = CultureInfo.CurrentCulture.TwoLetterISOLanguageName; ;
        }
    }
}
