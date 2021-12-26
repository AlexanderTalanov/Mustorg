using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class CustomerRepositorySQL : IRepository<Customer>
    {
        private MusicShop dataBase;

        public CustomerRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Customer> GetList()
        {
            return dataBase.Customers.ToList();
        }

        public Customer GetItem(int id)
        {
            return dataBase.Customers.Find(id);
        }

        public void Create(Customer item)
        {
            dataBase.Customers.Add(item);
        }

        public void Update(Customer item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Customer item = dataBase.Customers.Find(id);
            if (item != null)
            {
                dataBase.Customers.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
