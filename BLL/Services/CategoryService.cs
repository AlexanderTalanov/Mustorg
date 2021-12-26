using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        IDbRepository dataBase;
        public CategoryService(IDbRepository dbRepository)
        {
            dataBase = dbRepository;
        }

        public List<TypeModel> GetTypeModels()
        {
            var Types = dataBase.ProductTypes.GetList().Select(i => new TypeModel { Id = i.Id, Name = i.Name }).ToList();
            foreach (var type in Types)
            {
                var categories = dataBase.Categories.GetList().Where(i => i.Product_Type_Id == type.Id).Select(i => new CategoryModel { Id = i.Id, Name = i.Name, Product_Type_Id = i.Product_Type_Id} ).ToList();
                type.Categories = new System.Collections.ObjectModel.ObservableCollection<CategoryModel>();
                foreach (var cat in categories)
                {
                    type.Categories.Add(cat);
                }            
            }
            return Types;
        }
    }
}
