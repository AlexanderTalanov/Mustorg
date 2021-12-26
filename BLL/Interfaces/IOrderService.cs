using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BLL.Models;
using DAL.Interfaces;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        List<CartModel> GetShoppingCart(int customerId);
        decimal GetStatusSale(int id);
        int MakeOrder(int userId, int couponId, int pickPointId, decimal sum, ObservableCollection<CartModel> cart);
    }
}
