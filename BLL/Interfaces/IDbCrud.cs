using System.Collections.Generic;
using BLL.Models;

namespace BLL.Interfaces
{    
    public interface IDbCrud
    {
        List<ProductModel> GetAllProducts();
        List<BrandModel> GetAllBrands();
        List<CategoryModel> GetAllCategories();
        List<PickPointModel> GetPickPoints();
        ProductModel GetProduct(int id);
        ManagerModel GetManager(int id);
        List<StatusModel> GetAllStatuses();
        List<OrderModel> GetAllOrders();
        void UpdateProduct(ProductModel product);
        void DeleteProduct(int id);
        void DeleteCart(int id);
        void CreateProduct(ProductModel product);
        void CreateCart(CartModel cart);
        void CreateCustomer(CustomerModel customer);
        void UpdateCart(CartModel cart);
        void UpdateOrder(OrderModel orderModel);
        bool Save();
    }
}
