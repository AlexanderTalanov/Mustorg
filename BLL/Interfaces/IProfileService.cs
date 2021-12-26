using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IProfileService
    {
        List<OrderModel> GetCustomerOrders(int customerId, int statusId, DateTime dateStart, DateTime dateEnd);
        CustomerModel GetCutomerProfile(int customerId);
        List<CouponModel> GetCoupons(int customerId, decimal total);
        List<decimal> GetStats(int customerId, string year);
        bool CheckLogin(string login);
        bool CheckPassword(string login, string password);
        int GetUserId(string login);
    }
}
