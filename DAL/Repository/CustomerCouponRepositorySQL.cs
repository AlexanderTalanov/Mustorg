using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class CustomerCouponRepositorySQL : IRepository<Customer_Coupon>
    {
        private MusicShop dataBase;

        public CustomerCouponRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Customer_Coupon> GetList()
        {
            return dataBase.Customer_Coupons.ToList();
        }

        public Customer_Coupon GetItem(int id)
        {
            return dataBase.Customer_Coupons.Find(id);
        }

        public void Create(Customer_Coupon item)
        {
            dataBase.Customer_Coupons.Add(item);
        }

        public void Update(Customer_Coupon item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Customer_Coupon item = dataBase.Customer_Coupons.Find(id);
            if (item != null)
            {
                dataBase.Customer_Coupons.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
