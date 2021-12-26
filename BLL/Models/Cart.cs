using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace BLL.Models
{
    public class CartModel
    {
        public CartModel() { }
        public CartModel(Shopping_Cart shopping_Cart)
        {
            Id = shopping_Cart.Id;
            Amount = shopping_Cart.Amount;
            CustomerId = shopping_Cart.Customer_Id;
            ProductId = shopping_Cart.Product_Id;
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
        public string ViewPrice { get; set; }
        public string ViewSale { get; set; }
        public string ViewTotal { get; set; }
        public string Photo { get; set; }
        public string Exists { get; set; }
        public decimal? FullPrice { get; set; }
        public decimal? FullSale { get; set; }
        public bool MinusEnabled { get; set; }
        public bool PlusEnabled { get; set; }
    }
}
