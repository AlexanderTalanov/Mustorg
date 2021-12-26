using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class ManagerRepositorySQL : IRepository<Manager>
    {
        private MusicShop dataBase;

        public ManagerRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Manager> GetList()
        {
            return dataBase.Managers.ToList();
        }

        public Manager GetItem(int id)
        {
            return dataBase.Managers.Find(id);
        }

        public void Create(Manager item)
        {
            dataBase.Managers.Add(item);
        }

        public void Update(Manager item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Manager item = dataBase.Managers.Find(id);
            if (item != null)
            {
                dataBase.Managers.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
