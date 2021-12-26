using DAL.EF;

namespace DAL.Interfaces
{
    public interface IDbRepository
    {
        IRepository<Activity> Activities { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Category> Categories { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Customer_Status> CustomerStatuses { get; }
        IRepository<Manager> Managers { get; }
        IRepository<Order> Orders { get; }
        IRepository<Order_Line> OrderLines { get; }
        IRepository<Order_Status> OrderStatuses { get; }
        IRepository<Pick_Point> PickPoints { get; }
        IRepository<Product> Products { get; }
        IRepository<Product_Type> ProductTypes { get; }
        IRepository<Shopping_Cart> ShoppingCarts { get; }
        IRepository<Coupon> Coupons { get; }
        IRepository<Customer_Coupon> CustomerCoupons { get; }
        IServiceRepository Services { get; }
        int Save();
    }
}
