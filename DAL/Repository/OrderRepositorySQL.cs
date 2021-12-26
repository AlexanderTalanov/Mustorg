using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class OrderRepositorySQL : IRepository<Order>
    {
        private MusicShop dataBase;

        public OrderRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Order> GetList()
        {
            return dataBase.Orders.ToList();
        }

        public Order GetItem(int id)
        {
            return dataBase.Orders.Find(id);
        }

        public void Create(Order item)
        {
            dataBase.Orders.Add(item);
        }

        public void Update(Order item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Order item = dataBase.Orders.Find(id);
            if (item != null)
            {
                dataBase.Orders.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
