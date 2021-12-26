using DAL.EF;

namespace BLL.Models
{
    public class CategoryModel
    {
        public CategoryModel() { }

        public CategoryModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Product_Type_Id = category.Product_Type_Id;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Product_Type_Id { get; set; }

    }
}
