using AdaKurumsal.DataLayer;
using AdaKurumsal.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdaKurumsal.Pages.Admin.ProductManagement
{
    public class CategoriesModel : PageModel
    {
        private readonly IProductManagementDataService _productDataService;


        public CategoriesModel(IProductManagementDataService productDataService)
        {
            _productDataService = productDataService;
        }

        [BindProperty]
        public CategoryListModel Categories { get; set; } = new CategoryListModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Categories.TR = await _productDataService.GetAllTopLevelCategoriesWithSubCategoriesAsync("tr");
            Categories.EN = await _productDataService.GetAllTopLevelCategoriesWithSubCategoriesAsync("en");

            return Page();
        }
    }
}
