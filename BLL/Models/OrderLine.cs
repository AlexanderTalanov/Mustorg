using DAL.EF;

namespace BLL.Models
{
    public class Order_LineModel
    {
        public Order_LineModel() { }

        public Order_LineModel(Order_Line o_line)
        {
            Product_Id = o_line.Id;
            Order_Id = o_line.Id;
            Price = o_line.Price;
            Amount = o_line.Amount;
        }

        public int Id { get; set; }

        public int Product_Id { get; set; }

        public int Order_Id { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public string ViewArticle { get; set; }

        public string ViewAmount { get; set; }

        public string ViewPrice { get; set; }

        public string Photo { get; set; }

        public string Name { get; set; }

    }
}
