using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class ActivityRepositorySQL : IRepository<Activity>
    {
        private MusicShop dataBase;

        public ActivityRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Activity> GetList()
        {
            return dataBase.Activities.ToList();
        }

        public Activity GetItem(int id)
        {
            return dataBase.Activities.Find(id);
        }

        public void Create(Activity item)
        {
            dataBase.Activities.Add(item);
        }

        public void Update(Activity item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Activity item = dataBase.Activities.Find(id);
            if (item != null)
            {
                dataBase.Activities.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
