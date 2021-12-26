using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface ICatalogService
    {
        List<ProductModel> GetProductWithFilters(string category, int brandId, string text, int sort);
        List<ProductModel> GetTopFiveProducts(int userId);
    }
}
