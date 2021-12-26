using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class OrderStatusRepositorySQL : IRepository<Order_Status>
    {
        private MusicShop dataBase;

        public OrderStatusRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Order_Status> GetList()
        {
            return dataBase.Order_Statuses.ToList();
        }

        public Order_Status GetItem(int id)
        {
            return dataBase.Order_Statuses.Find(id);
        }

        public void Create(Order_Status item)
        {
            dataBase.Order_Statuses.Add(item);
        }

        public void Update(Order_Status item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Order_Status item = dataBase.Order_Statuses.Find(id);
            if (item != null)
            {
                dataBase.Order_Statuses.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
