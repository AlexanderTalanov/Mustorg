using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class OrderData
    {
        public decimal? Sum { get; set; }
    }

    public class ProductCatalogData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public decimal? Price { get; set; }
        public decimal? Sale { get; set; }
        public int Amount { get; set; }
    }

    public class CartData
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
        public string Photo { get; set; }
        public decimal? FullPrice { get; set; }
        public decimal? FullSale { get; set; }
    }

    public class CouponData
    {
        public int Id { get; set; }

        public int? Customer_Id { get; set; }

        public int? Coupon_Id { get; set; }

        public int? Order_Id { get; set; }
        
        public bool Used { get; set; }

        public string CouponName { get; set; }

        public decimal? Offer { get; set; }

        public decimal? Condition { get; set; }

        public string Background { get; set; }
    }

    public class CategoryData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Product_Type_Id { get; set; }
    }
}

