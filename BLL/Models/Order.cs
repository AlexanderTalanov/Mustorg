using System;
using System.Collections.ObjectModel;
using DAL.EF;

namespace BLL.Models
{
    public class OrderModel
    {
        public OrderModel() { }

        public OrderModel(Order order)
        {
            Order_Status_Id = order.Order_Status_Id;
            Pick_Point_Id = order.Pick_Point_Id;
            Id = order.Id;
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal? Sum { get; set; }

        public string ViewDate { get; set; }

        public string ViewSum { get; set; }

        public string ViewId { get; set; }

        public int? Order_Status_Id { get; set; }

        public int? Pick_Point_Id { get; set; }

        public string OrderStatus { get; set; }

        public string PickPoint { get; set; }

        public string Customer { get; set; }

        public string ManButton { get; set; }

        public ObservableCollection<Order_LineModel> OrderLines { get; set; }

    }
}
