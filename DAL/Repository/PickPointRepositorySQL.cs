using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class PickPointRepositorySQL : IRepository<Pick_Point>
    {
        private MusicShop dataBase;

        public PickPointRepositorySQL(MusicShop dbcontext)
        {
            this.dataBase = dbcontext;
        }
        public List<Pick_Point> GetList()
        {
            return dataBase.Pick_Points.ToList();
        }

        public Pick_Point GetItem(int id)
        {
            return dataBase.Pick_Points.Find(id);
        }

        public void Create(Pick_Point item)
        {
            dataBase.Pick_Points.Add(item);
        }

        public void Update(Pick_Point item)
        {
            dataBase.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Pick_Point item = dataBase.Pick_Points.Find(id);
            if (item != null)
            {
                dataBase.Pick_Points.Remove(item);
            }
        }

        public bool Save()
        {
            return dataBase.SaveChanges() > 0;
        }
    }
}
