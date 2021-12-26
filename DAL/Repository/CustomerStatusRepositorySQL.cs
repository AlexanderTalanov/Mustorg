using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class CustomerStatusRepositorySQL : IRepository<Customer_Status>
    {
        private MusicShop dataBase;

        public CustomerStatusRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Customer_Status> GetList()
        {
            return dataBase.Customer_Statuses.ToList();
        }

        public Customer_Status GetItem(int id)
        {
            return dataBase.Customer_Statuses.Find(id);
        }

        public void Create(Customer_Status item)
        {
            dataBase.Customer_Statuses.Add(item);
        }

        public void Update(Customer_Status item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Customer_Status item = dataBase.Customer_Statuses.Find(id);
            if (item != null)
            {
                dataBase.Customer_Statuses.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
