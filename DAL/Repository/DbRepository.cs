using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repository
{
    public class DbRepositorySQL : IDbRepository
    {
        private MusicShop dataBase;

        private BrandRepositorySQL brandRepository;
        private CategoryRepositorySQL categoryRepository;
        private CustomerRepositorySQL customerRepository;
        private CustomerStatusRepositorySQL customerStatusRepository;
        private ManagerRepositorySQL managerRepository;
        private OrderLineRepositorySQL orderLineRepository;
        private OrderRepositorySQL orderRepository;
        private OrderStatusRepositorySQL orderStatusRepository;
        private PickPointRepositorySQL pickPointRepository;
        private ProductRepositorySQL productRepository;
        private ProductTypeRepositorySQL productTypeRepository;
        private ServiceRepositorySQL serviceRepository;
        private ShoppingCartRepositorySQL shoppingCartRepository;
        private CouponRepositorySQL couponRepository;
        private CustomerCouponRepositorySQL customerCouponRepository;
        private ActivityRepositorySQL activityRepository;


        public DbRepositorySQL()
        {
            dataBase = new MusicShop();
        }

        public IRepository<Brand> Brands
        {
            get
            {
                if (brandRepository == null)
                    brandRepository = new BrandRepositorySQL(dataBase);
                return brandRepository;
            }
        }
        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepositorySQL(dataBase);
                return categoryRepository;
            }
        }
        public IRepository<Customer> Customers
        {
            get
            {
                if (customerRepository == null)
                    customerRepository = new CustomerRepositorySQL(dataBase);
                return customerRepository;
            }
        }
        public IRepository<Customer_Status> CustomerStatuses
        {
            get
            {
                if (customerStatusRepository == null)
                    customerStatusRepository = new CustomerStatusRepositorySQL(dataBase);
                return customerStatusRepository;
            }
        }
        public IRepository<Manager> Managers
        {
            get
            {
                if (managerRepository == null)
                    managerRepository = new ManagerRepositorySQL(dataBase);
                return managerRepository;
            }
        }
        public IRepository<Order_Line> OrderLines
        {
            get
            {
                if (orderLineRepository == null)
                    orderLineRepository = new OrderLineRepositorySQL(dataBase);
                return orderLineRepository;
            }
        }
        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepositorySQL(dataBase);
                return orderRepository;
            }
        }
        public IRepository<Order_Status> OrderStatuses
        {
            get
            {
                if (orderStatusRepository == null)
                    orderStatusRepository = new OrderStatusRepositorySQL(dataBase);
                return orderStatusRepository;
            }
        }
        public IRepository<Pick_Point> PickPoints
        {
            get
            {
                if (pickPointRepository == null)
                    pickPointRepository = new PickPointRepositorySQL(dataBase);
                return pickPointRepository;
            }
        }
        public IRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepositorySQL(dataBase);
                return productRepository;
            }
        }
        public IRepository<Product_Type> ProductTypes
        {
            get
            {
                if (productTypeRepository == null)
                    productTypeRepository = new ProductTypeRepositorySQL(dataBase);
                return productTypeRepository;
            }
        }
        public IRepository<Shopping_Cart> ShoppingCarts
        {
            get
            {
                if (shoppingCartRepository == null)
                    shoppingCartRepository = new ShoppingCartRepositorySQL(dataBase);
                return shoppingCartRepository;
            }
        }
        public IRepository<Coupon> Coupons
        {
            get
            {
                if (couponRepository == null)
                    couponRepository = new CouponRepositorySQL(dataBase);
                return couponRepository;
            }
        }
        public IRepository<Customer_Coupon> CustomerCoupons
        {
            get
            {
                if (customerCouponRepository == null)
                    customerCouponRepository = new CustomerCouponRepositorySQL(dataBase);
                return customerCouponRepository;
            }
        }

        public IRepository<Activity> Activities
        {
            get
            {
                if (activityRepository == null)
                    activityRepository = new ActivityRepositorySQL(dataBase);
                return activityRepository;
            }
        }

        public IServiceRepository Services
        {
            get 
            {
                if (serviceRepository == null)
                    serviceRepository = new ServiceRepositorySQL(dataBase);
                return serviceRepository;
            }
        }
        public int Save()
        {
            return dataBase.SaveChanges();
        }
    }
}
