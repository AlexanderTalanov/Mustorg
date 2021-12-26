using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace DAL.Interfaces
{
    public interface IServiceRepository
    {
        List<ProductCatalogData> GetProductWithFilters(string category, int brandId, string text, int sort);
        List<CartData> GetShoppingCart(int userId);
        List<CouponData> GetCoupons(int userId);
        bool CheckLogin(string login);
        bool CheckPassword(string login, string password);
        int GetUserId(string login);
        bool CheckManLogin(string login);
        bool CheckManPassword(string login, string password);
        int GetManUserId(string login);
        List<ProductCatalogData> GetTopFiveProducts(int customerId);
    }
}
