using DAL.EF;
using System.ComponentModel;

namespace BLL.Models
{
    public class ProductModel : INotifyPropertyChanged
    {
        public ProductModel() { }
        public ProductModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Amount = product.Amount;
            Price = product.Price;
            Sale = product.Sale;
            Description = product.Description;
            Availability = product.Availability;
            Guarantee = product.Guarantee;
            Article = product.Article;
            Brand_Id = product.Brand_Id;
            Category_Id = product.Category_Id;
            Photo = product.Photo;
        }

        public string Name { get; set; }

        public int Amount { get; set; }

        public decimal? Price { get; set; }

        public string ViewPrice { get; set; }

        public decimal? Sale { get; set; }

        public string Description { get; set; }

        public bool Availability { get; set; }

        public int Id { get; set; }

        public string Guarantee { get; set; }

        public int Article { get; set; }

        public int Brand_Id { get; set; }

        public int Category_Id { get; set; }

        public string Photo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
