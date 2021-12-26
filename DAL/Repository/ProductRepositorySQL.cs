using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class ProductRepositorySQL : IRepository<Product>
    {
        private MusicShop dataBase;

        public ProductRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Product> GetList()
        {
            return dataBase.Products.ToList();
        }

        public Product GetItem(int id)
        {
            return dataBase.Products.Find(id);
        }

        public void Create(Product item)
        {
            dataBase.Products.Add(item);
        }

        public void Update(Product item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Product item = dataBase.Products.Find(id);
            if (item != null)
            {
                dataBase.Products.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
