using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using System.Collections.ObjectModel;
using DAL.Interfaces;

namespace BLL.Services
{
    public class ProfileService : IProfileService
    {
        IDbRepository dataBase;
        public ProfileService(IDbRepository dbRepository)
        {
            dataBase = dbRepository;
        }
        public List<OrderModel> GetCustomerOrders(int customerId, int statusId, DateTime dateStart, DateTime dateEnd)
        {
            var orders = dataBase.Orders.GetList();
            var orderLines = dataBase.OrderLines.GetList();

            List<OrderModel> orderModels = orders.Where(i => customerId == i.Customer_Id && dateStart <= i.Date && dateEnd >= i.Date && ((statusId == -1 && (i.Order_Status_Id == 1 | i.Order_Status_Id == 2 || i.Order_Status_Id == 3)) | statusId == 0 | statusId == i.Order_Status_Id)).Select(i => new OrderModel { OrderLines = new ObservableCollection<Order_LineModel>(), Id = i.Id, Date = (DateTime)i.Date, Order_Status_Id = i.Order_Status_Id, Pick_Point_Id = i.Pick_Point_Id, Sum = i.Sum  }).ToList();

            foreach (var i in orderModels)
            {
                foreach (var j in orderLines)
                {
                    if (j.Order_Id == i.Id)
                    {
                        var prod = dataBase.Products.GetItem(j.Product_Id);
                        i.OrderLines.Add(new Order_LineModel { ViewAmount = $"Количество: {j.Amount} шт.", ViewPrice = $"Стоимость: {j.Price:0.#} руб." ,Price = j.Price, Amount = j.Amount, Photo = prod.Photo, Name = prod.Name });
                    }
                }
                i.OrderStatus = dataBase.OrderStatuses.GetItem((int)i.Order_Status_Id).Name;
                i.PickPoint = dataBase.PickPoints.GetItem((int)i.Pick_Point_Id).Name;
            }

            return orderModels;
        }
        public List<decimal> GetStats(int customerId, string year)
        {
            var orders = dataBase.Orders.GetList().Where(i => ((DateTime)i.Date).Year.ToString() == year && customerId == i.Customer_Id).ToList();

            List<decimal> months = new List<decimal>();
            for (int i = 0; i < 12; i++)
            {
                decimal sum = 0;
                foreach (var j in orders)
                {
                    if (((DateTime)j.Date).Month.ToString() == (i + 1).ToString()) sum += (decimal)j.Sum;
                }
                months.Add(sum);
            }

            return months;
        }

        public List<CouponModel> GetCoupons(int customerId, decimal total)
        {
            return dataBase.Services.GetCoupons(customerId).Where(i => i.Used == false && i.Condition <= total).Select(i => new CouponModel { Background = i.Background, Condition = i.Condition, CouponId = i.Coupon_Id, CouponName = i.CouponName, CustomerId = i.Customer_Id, Id = i.Id, Offer = i.Offer, OrderId = i.Order_Id, Used = i.Used}).ToList();
        }
        public int GetUserId(string login)
        {
            return dataBase.Services.GetUserId(login);
        }
        public bool CheckLogin(string login)
        {
            return dataBase.Services.CheckLogin(login);
        }

        public CustomerModel GetCutomerProfile(int customerId)
        {
            var customer = dataBase.Customers.GetItem(customerId);
            decimal? sum = 0;

            var orders = dataBase.Orders.GetList().Where(i => i.Customer_Id == customerId);
            foreach (var i in orders)
            {
                sum += i.Sum;
            }

            var status = dataBase.CustomerStatuses.GetItem(customer.Customer_Status_Id);
            double progress = 0;
            if (sum != 0 & status.Sum != 0)
            {
                progress = Convert.ToDouble(sum / status.Sum);
            }

            return new CustomerModel
            {
                Name = customer.Name,
                Progress = progress * 100,
                Sum = sum,
                Customer_Status_Id = customer.Customer_Status_Id            
            };
        }

        public bool CheckPassword(string login, string password)
        {
            return dataBase.Services.CheckPassword(login, password);
        }
    }
}
