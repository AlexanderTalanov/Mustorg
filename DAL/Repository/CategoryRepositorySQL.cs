using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class CategoryRepositorySQL : IRepository<Category>
    {
        private MusicShop dataBase;

        public CategoryRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Category> GetList()
        {
            return dataBase.Categories.ToList();
        }

        public Category GetItem(int id)
        {
            return dataBase.Categories.Find(id);
        }

        public void Create(Category item)
        {
            dataBase.Categories.Add(item);
        }

        public void Update(Category item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Category item = dataBase.Categories.Find(id);
            if (item != null)
            {
                dataBase.Categories.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
