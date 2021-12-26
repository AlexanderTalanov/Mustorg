using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class OrderLineRepositorySQL : IRepository<Order_Line>
    {
        private MusicShop dataBase;

        public OrderLineRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Order_Line> GetList()
        {
            return dataBase.Order_Lines.ToList();
        }

        public Order_Line GetItem(int id)
        {
            return dataBase.Order_Lines.Find(id);
        }

        public void Create(Order_Line item)
        {
            dataBase.Order_Lines.Add(item);
        }

        public void Update(Order_Line item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Order_Line item = dataBase.Order_Lines.Find(id);
            if (item != null)
            {
                dataBase.Order_Lines.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
