using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IManagerService
    {
        bool CheckLogin(string login);
        bool CheckPassword(string login, string password);
        int GetUserId(string login);
        List<OrderModel> GetOrdersWarehouse(DateTime start, DateTime end, int statusType, int pickPoint, string text);
    }
}
