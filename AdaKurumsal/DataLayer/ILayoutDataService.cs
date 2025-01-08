using AdaKurumsal.Models.PageModels;

namespace AdaKurumsal.DataLayer
{
    public interface ILayoutDataService
    {
        Task<LayoutModel> GetLayout();
    }
}
