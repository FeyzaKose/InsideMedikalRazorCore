using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdaKurumsal.Pages
{
    public class LangModel : PageModel
    {
        public void OnGet()
        {
            string? culture = Request.Query["culture"];
            if (culture != null)
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTime.Now.AddDays(1) }
                    );
            }

            string returnUrl = Request.Headers["Referer"].ToString() ?? "/";

            Response.Redirect(returnUrl);
        }
    }
}
