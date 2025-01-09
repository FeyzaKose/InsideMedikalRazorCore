namespace AdaKurumsal.Models.DataModels
{
    public class Category : BaseModel
    {
        public string MainCategoryCode { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        public bool isActive { get; set; }

    }
}
