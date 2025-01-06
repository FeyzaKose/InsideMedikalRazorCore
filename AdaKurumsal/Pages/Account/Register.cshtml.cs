using AdaKurumsal.DataLayer;
using AdaKurumsal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdaKurumsal.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public AppUser user { get; set; }


        public string passwordConfirm { get; set; }

        protected EFContext context;
        public RegisterModel(EFContext _context)
        {
            this.context = _context;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(AppUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                else
                {
                    user.Password = Tools.Kit.HashString(user.Password);
                    context.AppUsers.AddAsync(user);
                    context.SaveChanges();
                    return Page();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToPage("Index");
        }
    }
}
