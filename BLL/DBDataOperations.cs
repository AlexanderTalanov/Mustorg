using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Interfaces;
using BLL.Interfaces;
using DAL.EF;
using BLL.Models;
using System.ComponentModel;

namespace BLL
{
    public class DBDataOperations : IDbCrud
    {
        IDbRepository dataBase;

        public DBDataOperations(IDbRepository repos)
        {
            dataBase = repos;
        }

        public List<ProductModel> GetAllProducts()
        {
            var temp = dataBase.Products.GetList().Select(i => new ProductModel(i)).ToList();
        
            foreach (var i in temp)
            {
                i.Sale = NewYearSale(i.Brand_Id, i.Category_Id, i.Price, i.Sale);
            }
            return temp;
        }

        public List<BrandModel> GetAllBrands()
        {
            return dataBase.Brands.GetList().Select(i => new BrandModel(i)).ToList();
        }

        public List<StatusModel> GetAllStatuses()
        {
            return dataBase.OrderStatuses.GetList().Select(i => new StatusModel(i)).ToList();
        }

        public List<OrderModel> GetAllOrders()
        {
            return dataBase.Orders.GetList().Select(i => new OrderModel(i)).ToList();
        }

        public List<CategoryModel> GetAllCategories()
        {
            return dataBase.Categories.GetList().Select(i => new CategoryModel(i)).ToList();
        }

        public List<PickPointModel> GetPickPoints()
        {
            return dataBase.PickPoints.GetList().Select(i => new PickPointModel(i)).ToList();
        }

        public ProductModel GetProduct(int id)
        {
            var result = dataBase.Products.GetItem(id);
            ProductModel prod = new ProductModel();
            prod.Amount = result.Amount;
            return prod;
        }

        public ManagerModel GetManager(int id)
        {
            Manager man = dataBase.Managers.GetItem(id);
            ManagerModel newMan = new ManagerModel(man);
            newMan.Pick_Point_Name = newMan.Pick_Point_Id != 0 ? dataBase.PickPoints.GetItem(newMan.Pick_Point_Id).Name : null;
            newMan.Activity_Name = dataBase.Activities.GetItem(newMan.Activity_Id).Name;
            return newMan;
        }

        public void CreateCustomer(CustomerModel customer)
        {
            Customer cust = new Customer();
            cust.Customer_Status_Id = 4;
            cust.Sum = 0;
            cust.Login = customer.Login;
            cust.Name = customer.Name;
            cust.Password = customer.Password;
            cust.Email = customer.Email;         

            dataBase.Customers.Create(cust);
            Save();

            int cuid = dataBase.Customers.GetList().Where(i => i.Login == customer.Login).ToList()[0].Id;
            Customer_Coupon coupon = new Customer_Coupon();
            coupon.Customer_Id = cuid;
            coupon.Coupon_Id = 4;
            coupon.Used = false;

            dataBase.CustomerCoupons.Create(coupon);
            Save();
        }

        public void CreateCart(CartModel cart)
        {
            Shopping_Cart sc = new Shopping_Cart();
            var allcart = dataBase.ShoppingCarts.GetList().Where(i => i.Customer_Id == cart.CustomerId && i.Product_Id == cart.ProductId).ToList();
            if (allcart.Count != 0) return;

            sc.Amount = 1;
            sc.Customer_Id = cart.CustomerId;
            sc.Product_Id = cart.ProductId;
            dataBase.ShoppingCarts.Create(sc);
            Save();
        }

        public void UpdateProduct(ProductModel product)
        {
            Product prod = dataBase.Products.GetItem(product.Id);
            prod.Name = product.Name;
            prod.Amount = product.Amount;
            prod.Price = product.Price;
            prod.Sale = NewYearSale(product.Brand_Id, product.Category_Id, product.Price, product.Sale);
            prod.Description = product.Description;
            prod.Availability = product.Availability;
            prod.Guarantee = product.Guarantee;
            prod.Article = product.Article;
            prod.Brand_Id = product.Brand_Id;
            prod.Category_Id = product.Category_Id;
            Save();
        }

        public void UpdateOrder(OrderModel orderModel)
        {
            Order order = dataBase.Orders.GetItem(orderModel.Id);
            order.Order_Status_Id = orderModel.Order_Status_Id;
            Save();
        }

        public void UpdateCart(CartModel cart)
        {
            Shopping_Cart sc = dataBase.ShoppingCarts.GetItem(cart.Id);
            sc.Amount = cart.Amount;
            Save();
        }

        public void DeleteProduct(int id)
        {
            Product product = dataBase.Products.GetItem(id);
            if (product != null)
            {
                dataBase.Products.Delete(product.Id);
                Save();
            }
        }

        public void DeleteCart(int id)
        {
            Shopping_Cart cart = dataBase.ShoppingCarts.GetItem(id);
            if (cart != null)
            {
                dataBase.ShoppingCarts.Delete(cart.Id);
                Save();
            }
        }

        public void CreateProduct(ProductModel product)
        {
            dataBase.Products.Create(new Product()
            {
                Name = product.Name,
                Amount = product.Amount,
                Price = product.Price,
                Sale = product.Sale,
                Description = product.Description,
                Availability = product.Availability,
                Id = product.Id,
                Guarantee = product.Guarantee,
                Article = product.Article,
                Brand_Id = product.Brand_Id,
                Category_Id = product.Category_Id,
            });
            Save();
        }

        private decimal? NewYearSale(int brandId, int categoryId, decimal? price, decimal? sale)
        {
            if (brandId == 2 & (categoryId == 2 | categoryId == 3)) //Fender
            {
                return price * 0.1m;
            }
            else
            {
                return sale;
            }
        }

        public bool Save()
        {
            if (dataBase.Save() > 0) return true;
            return false;
        }
    }
}
