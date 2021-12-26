using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class CouponRepositorySQL : IRepository<Coupon>
    {
        private MusicShop dataBase;

        public CouponRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Coupon> GetList()
        {
            return dataBase.Coupons.ToList();
        }

        public Coupon GetItem(int id)
        {
            return dataBase.Coupons.Find(id);
        }

        public void Create(Coupon item)
        {
            dataBase.Coupons.Add(item);
        }

        public void Update(Coupon item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Coupon item = dataBase.Coupons.Find(id);
            if (item != null)
            {
                dataBase.Coupons.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
