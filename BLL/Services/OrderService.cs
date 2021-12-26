using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using DAL.EF;
using DAL.Interfaces;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        IDbRepository dataBase;
        public OrderService(IDbRepository dbRepository)
        {
            dataBase = dbRepository;
        }
        public decimal GetStatusSale(int id)
        {
            var result = dataBase.Customers.GetItem(id);
            return dataBase.CustomerStatuses.GetItem(result.Customer_Status_Id).Bonus;
        }

        public int MakeOrder(int userId, int couponId, int pickPointId, decimal sum, ObservableCollection<CartModel> cart)
        {
            try
            {
                decimal sale = sum * GetStatusSale(userId);
                DAL.EF.Order order = new DAL.EF.Order();
                order.Customer_Id = userId;
                order.Date = DateTime.Now.Date;
                order.Order_Status_Id = 3; //Обрабатывается
                order.Pick_Point_Id = pickPointId;
                order.Sum = sum - sale;
                order.Sale = sale;
                dataBase.Orders.Create(order);

                if (dataBase.Save() <= 0)
                    return 0;

                var orders = dataBase.Orders.GetList();
                int key = orders[orders.Count - 1].Id;
                foreach (var i in cart)
                {
                    DAL.EF.Order_Line line = new DAL.EF.Order_Line();
                    line.Product_Id = i.ProductId;
                    line.Order_Id = key;
                    line.Amount = i.Amount;
                    line.Price = ((decimal)i.FullPrice - (decimal)i.FullSale) * (1 - GetStatusSale(userId));
                    Product prod = dataBase.Products.GetItem(i.ProductId);
                    prod.Amount -= i.Amount;
                    if (prod.Amount == 0) prod.Availability = false;
                    dataBase.Products.Update(prod);                    
                    dataBase.OrderLines.Create(line);
                }

                if (couponId != 0)
                {
                    DAL.EF.Customer_Coupon coupon = dataBase.CustomerCoupons.GetItem(couponId);
                    coupon.Used = true;
                    coupon.Order_Id = key;
                    dataBase.CustomerCoupons.Create(coupon);

                    if (dataBase.Save() <= 0)
                        return 0;
                }

                List<Customer_Status> status = dataBase.CustomerStatuses.GetList();
                Customer customer = dataBase.Customers.GetItem(userId);
                customer.Sum += sum - sale;
                bool flag = true;
                for (int i = 3; i > 0; i--)
                {
                    if (status[i].Sum >= customer.Sum)
                    {
                        customer.Customer_Status_Id = status[i].Id;
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    customer.Customer_Status_Id = 1;
                }

                if (dataBase.Save() <= 0)
                    return 0;

                var carts = dataBase.ShoppingCarts.GetList();
                for (int j = 0; j < carts.Count; j++)
                {
                    if (carts[j].Customer_Id == userId)
                    {
                        dataBase.ShoppingCarts.Delete(carts[j].Id);
                    }
                }

                GiveAwayCoupons(sum - sale, userId);

                return 1;
            }
            finally
            {
                
            }
        }


        public List<CartModel> GetShoppingCart(int customerId)
        {
            List<CartModel> cart = dataBase.Services.GetShoppingCart(customerId).Select(i => new CartModel { Amount = i.Amount, CustomerId = i.CustomerId, FullPrice = i.FullPrice, FullSale = i.FullSale, Id = i.Id, Photo = i.Photo, ProductId = i.ProductId, ProductName = i.ProductName }).ToList();
            foreach (var i in cart)
            {
                if (dataBase.Products.GetItem(i.ProductId).Amount >= i.Amount)
                {
                    i.Exists = "Hidden";
                }
                else 
                {
                    i.Exists = "Visible";
                }
            }

            return cart;
        }

        private void GiveAwayCoupons(decimal sum, int userId)
        {
            List<Coupon> coupons = dataBase.Coupons.GetList();

            decimal max = -1;
            int point = 0;
            for (int i = 0; i < coupons.Count; i++)
            {
                if (sum > coupons[i].GiveawayCondition && coupons[i].GiveawayCondition != -1 && coupons[i].GiveawayCondition > max)
                {
                    max = coupons[i].GiveawayCondition;
                    point = i;
                }
            }
            if (point != 0)
            {
                Customer_Coupon customer_Coupon = new Customer_Coupon();
                customer_Coupon.Coupon_Id = coupons[point].Id;
                customer_Coupon.Customer_Id = userId;
                customer_Coupon.Used = false;
                dataBase.CustomerCoupons.Create(customer_Coupon);
                dataBase.Save();
            }
        }
    }
}
