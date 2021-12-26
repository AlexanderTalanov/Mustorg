using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class ServiceRepositorySQL : IServiceRepository
    {
        private MusicShop dataBase;

        public ServiceRepositorySQL(MusicShop dbcontext)
        {
            dataBase = dbcontext;
        }

        public List<CartData> GetShoppingCart(int customerId)
        {
            MusicShop db = new MusicShop();

            var result = db.Shopping_Carts
                           .Where(i => i.Customer_Id == customerId)
                           .Join(db.Products, car => car.Product_Id, pr => pr.Id, (car, pr) => new CartData { Id = car.Id, ProductId = car.Product_Id, CustomerId = car.Customer_Id, Amount = car.Amount, FullPrice = car.Amount * pr.Price, FullSale = car.Amount * pr.Sale, ProductName = pr.Name, Photo = pr.Photo })
                           .ToList();                                      
            return result;
        }

        public List<CouponData> GetCoupons(int customerId)
        {
            MusicShop db = new MusicShop();

            var result = db.Customer_Coupons.Where(i => i.Customer_Id == customerId)
                           .Join(db.Coupons, cc => cc.Coupon_Id, co => co.Id, (cc, co) => new CouponData
                           {
                               Id = cc.Id,
                               CouponName = co.Name, 
                               Background = co.Background, 
                               Condition = co.Condition, Coupon_Id = co.Id, 
                               Customer_Id = cc.Customer_Id, Offer = co.Offer,
                               Order_Id = cc.Order_Id, 
                               Used = (bool)cc.Used
                           }).ToList();

            return result;
        }

        public List<ProductCatalogData> GetTopFiveProducts(int customerId)
        {
            MusicShop db = new MusicShop();

            List<ProductCatalogData> lines = db.Order_Lines.Join(db.Orders, ol => ol.Order_Id, o => o.Id, (ol, o) => new { o.Customer_Id, ol.Product_Id, ol.Amount })
                                      .Where(i => i.Customer_Id == customerId)
                                      .Join(db.Products, ol => ol.Product_Id, pr => pr.Id, (ol, pr) => new ProductCatalogData { Amount = ol.Amount, Id = pr.Id, Name = pr.Name, Photo = pr.Photo, Price = pr.Price, Sale = pr.Sale })
                                      .ToList();

            for(int i = 0; i < lines.Count; i++)
            {
                int j = i + 1;

                while (j < lines.Count)
                {
                    if (lines[i].Id == lines[j].Id)
                    {
                        lines[i].Amount += lines[j].Amount;
                        lines.RemoveAt(j);
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            lines.OrderBy(i => i.Amount);
            List<ProductCatalogData> result = new List<ProductCatalogData>();
            for (int i = 0; i < 5 & i < lines.Count; i++)
            {
                result.Add(lines[i]);
            }
            return result;
        }

        public List<ProductCatalogData> GetProductWithFilters(string category, int brandId, string text, int sort)
        {
            MusicShop db = new MusicShop();

            var temp = db.Products.ToList();
            List<ProductCatalogData> result;
            

            if (category == null)
            {
                result = temp.Where(i => (i.Brand_Id == brandId || brandId == 0) && (i.Name.ToLowerInvariant().Contains(text.ToLowerInvariant()) | (i.Article.ToString().Contains(text.ToLowerInvariant()))))
                                 .Select(i => new ProductCatalogData { Id = i.Id, Name = i.Name, Photo = i.Photo, Price = i.Price, Sale = i.Sale}).ToList();
            }
            else if (db.Product_Types.Where(i => i.Name == category).Count() != 0)
            {
                result = temp.Where(i => (i.Brand_Id == brandId || brandId == 0) && i.Name.ToLowerInvariant().Contains(text.ToLowerInvariant()) | (i.Article.ToString().Contains(text.ToLowerInvariant())))
                               .Join(db.Categories, pr => pr.Category_Id, cat => cat.Id, (pr, cat) => new { Id = pr.Id, CategName = cat.Name, TypeId = cat.Product_Type_Id, pr.Name, pr.Price, Sale = pr.Sale, pr.Photo, pr.Category_Id })
                               .Join(db.Product_Types, pr => pr.TypeId, prt => prt.Id, (pr, prt) => new { TypeName = prt.Name, Id = pr.Id, Name = pr.Name, Photo = pr.Photo, Price = pr.Price, Sale = pr.Sale})
                               .Where(i => i.TypeName == category)
                               .Select(i => new ProductCatalogData { Id = i.Id, Name = i.Name, Photo = i.Photo, Price = i.Price, Sale = i.Sale })
                               .ToList();
            }
            else
            { 
                result = temp.Where(i => (i.Brand_Id == brandId || brandId == 0) && i.Name.ToLowerInvariant().Contains(text.ToLowerInvariant()) | (i.Article.ToString().Contains(text.ToLowerInvariant())))
                               .Join(db.Categories, pr => pr.Category_Id, cat => cat.Id, (pr, cat) => new { Id = pr.Id, CategName = cat.Name, pr.Name, pr.Price, Sale = pr.Sale, pr.Photo })
                               .Where(i => i.CategName == category)
                               .Select(i => new ProductCatalogData { Id = i.Id, Name = i.Name, Photo = i.Photo, Price = i.Price, Sale = i.Sale })
                               .ToList();
            }

            if (sort == 0)
            {
                return result;
            }
            else if (sort == 1)
            {
                return result.OrderBy(i => i.Price).ToList();
            }
            else 
            {
                return result.OrderBy(i => -1 * i.Price).ToList();
            }
        }   

        public bool CheckLogin(string login)
        {
            MusicShop db = new MusicShop();

            if (db.Customers.Where(i => login == i.Login).Count() == 0) return false;
            else return true;
        }

        public bool CheckPassword(string login, string password)
        {
            MusicShop db = new MusicShop();

            if (db.Customers.Where(i => login == i.Login && password == i.Password).Count() == 0) return false;
            else return true;
        }

        public int GetUserId(string login)
        {
            return dataBase.Customers.Where(i => i.Login == login).ToList()[0].Id;
        }

        public bool CheckManLogin(string login)
        {
            MusicShop db = new MusicShop();

            if (db.Managers.Where(i => login == i.Login).Count() == 0) return false;
            else return true;
        }

        public bool CheckManPassword(string login, string password)
        {
            MusicShop db = new MusicShop();

            if (db.Managers.Where(i => login == i.Login && password == i.Password).Count() == 0) return false;
            else return true;
        }

        public int GetManUserId(string login)
        {
            return dataBase.Managers.Where(i => i.Login == login).ToList()[0].Id;
        }
    }
}
