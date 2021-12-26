using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class ShoppingCartRepositorySQL : IRepository<Shopping_Cart>
    {
        private MusicShop dataBase;

        public ShoppingCartRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Shopping_Cart> GetList()
        {
            return dataBase.Shopping_Carts.ToList();
        }

        public Shopping_Cart GetItem(int id)
        {
            return dataBase.Shopping_Carts.Find(id);
        }

        public void Create(Shopping_Cart item)
        {
            dataBase.Shopping_Carts.Add(item);
        }

        public void Update(Shopping_Cart item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Shopping_Cart item = dataBase.Shopping_Carts.Find(id);
            if (item != null)
            {
                dataBase.Shopping_Carts.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
