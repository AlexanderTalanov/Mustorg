using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Services
{
    public class ProductCatalogService : ICatalogService
    {
        IDbRepository dataBase;
        public ProductCatalogService(IDbRepository dbRepository)
        {
            dataBase = dbRepository;
        }

        public List<ProductModel> GetProductWithFilters(string category, int brandId, string text, int sort)
        {
            return dataBase.Services.GetProductWithFilters(category, brandId, text, sort).Select(i => new ProductModel { Id = i.Id, Photo = i.Photo, Name = i.Name, Sale = i.Sale, Price = i.Price }).ToList();
        }
        public List<ProductModel> GetTopFiveProducts(int userId)
        {
            return dataBase.Services.GetTopFiveProducts(userId).Select(i => new ProductModel { Id = i.Id, Photo = i.Photo, Name = i.Name, Sale = i.Sale, Price = i.Price }).ToList();
        }

    }
}
