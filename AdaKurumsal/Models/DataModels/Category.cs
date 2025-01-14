namespace AdaKurumsal.Models.DataModels
{
    public class Category : BaseModel
    {
        public string MainCategoryCode { get; set; }
        public string Code { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; }
        public bool isActive { get; set; }


        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    }
}
