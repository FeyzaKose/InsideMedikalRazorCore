using AdaKurumsal.Models.PageModels;

namespace AdaKurumsal.DataLayer
{
    public interface ILayoutDataService
    {
        Task<LayoutModel> GetLayout();
        Task RefreshLayout(string language);
    }
}
