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
    public class ManagerService : IManagerService
    {
        IDbRepository dataBase;
        public ManagerService(IDbRepository dbRepository)
        {
            dataBase = dbRepository;
        }

        public int GetUserId(string login)
        {
            return dataBase.Services.GetManUserId(login);
        }
        public bool CheckLogin(string login)
        {
            return dataBase.Services.CheckManLogin(login);
        }

        public bool CheckPassword(string login, string password)
        {
            return dataBase.Services.CheckManPassword(login, password);
        }

        public List<OrderModel> GetOrdersWarehouse(DateTime dateStart, DateTime dateEnd, int statusType, int pickPoint, string text)
        {
            //StatusType 0 - склад, 1 - пункт выдачи
            var orders = dataBase.Orders.GetList();
            var orderLines = dataBase.OrderLines.GetList();

            List<OrderModel> orderModels = orders.Where(i => dateStart <= i.Date && dateEnd >= i.Date && ((statusType == 0 && i.Order_Status_Id == 3) || (statusType == 1 && (i.Order_Status_Id == 2 || i.Order_Status_Id == 1) && i.Pick_Point_Id == pickPoint)) && i.Id.ToString().Contains(text)).Join(dataBase.Customers.GetList(), i => i.Customer_Id, j => j.Id, (i,j) => new OrderModel { OrderLines = new ObservableCollection<Order_LineModel>(), Id = i.Id, Date = (DateTime)i.Date, Order_Status_Id = i.Order_Status_Id, Pick_Point_Id = i.Pick_Point_Id, Sum = i.Sum, Customer = $"Покупатель: { j.Name }" }).ToList();

            foreach (var i in orderModels)
            {
                foreach (var j in orderLines)
                {
                    if (j.Order_Id == i.Id)
                    {
                        var prod = dataBase.Products.GetItem(j.Product_Id);
                        i.OrderLines.Add(new Order_LineModel { ViewAmount = $"Количество: {j.Amount} шт.", ViewArticle = $"Артукул: {prod.Article}", Name = prod.Name, Price = j.Price, Amount = j.Amount });
                    }
                }
                i.OrderStatus = dataBase.OrderStatuses.GetItem((int)i.Order_Status_Id).Name;
                i.PickPoint = dataBase.PickPoints.GetItem((int)i.Pick_Point_Id).Name;
            }

            return orderModels;
        }
    }
}
