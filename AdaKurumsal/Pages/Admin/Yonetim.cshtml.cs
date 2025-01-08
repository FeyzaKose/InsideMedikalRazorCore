using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdaKurumsal.Pages.Admin
{
    [Authorize]
    public class YonetimModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
