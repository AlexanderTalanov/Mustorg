using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.EF
{
    public partial class MusicShop : DbContext
    {
        public MusicShop()
            : base("name=MusicShop")
        {
        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Customer_Coupon> Customer_Coupons { get; set; }
        public virtual DbSet<Customer_Status> Customer_Statuses { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Order_Line> Order_Lines { get; set; }
        public virtual DbSet<Order_Status> Order_Statuses { get; set; }
        public virtual DbSet<Pick_Point> Pick_Points { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_Type> Product_Types { get; set; }
        public virtual DbSet<Shopping_Cart> Shopping_Carts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .HasMany(e => e.Manager)
                .WithOptional(e => e.Activity)
                .HasForeignKey(e => e.Activity_Id);

            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.Brand)
                .HasForeignKey(e => e.Brand_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.Category)
                .HasForeignKey(e => e.Category_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.Offer)
                .HasPrecision(9, 3);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.Condition)
                .HasPrecision(9, 3);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.Background)
                .IsFixedLength();

            modelBuilder.Entity<Coupon>()
                .Property(e => e.GiveawayCondition)
                .HasPrecision(9, 3);

            modelBuilder.Entity<Coupon>()
                .HasMany(e => e.Customer_Coupon)
                .WithOptional(e => e.Coupon)
                .HasForeignKey(e => e.Coupon_Id);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Sum)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Customer_Coupon)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.Customer_Id);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Order)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.Customer_Id);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Shopping_Cart)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.Customer_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer_Status>()
                .Property(e => e.Bonus)
                .HasPrecision(2, 2);

            modelBuilder.Entity<Customer_Status>()
                .Property(e => e.Sum)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Customer_Status>()
                .HasMany(e => e.Customer)
                .WithRequired(e => e.Customer_Status)
                .HasForeignKey(e => e.Customer_Status_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Sum)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order>()
                .Property(e => e.Sale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Customer_Coupon)
                .WithOptional(e => e.Order)
                .HasForeignKey(e => e.Order_Id);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Order_Line)
                .WithRequired(e => e.Order)
                .HasForeignKey(e => e.Order_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order_Line>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order_Status>()
                .HasMany(e => e.Order)
                .WithOptional(e => e.Order_Status)
                .HasForeignKey(e => e.Order_Status_Id);

            modelBuilder.Entity<Pick_Point>()
                .HasMany(e => e.Manager)
                .WithOptional(e => e.Pick_Point)
                .HasForeignKey(e => e.Pick_Point_Id);

            modelBuilder.Entity<Pick_Point>()
                .HasMany(e => e.Order)
                .WithOptional(e => e.Pick_Point)
                .HasForeignKey(e => e.Pick_Point_Id);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.Sale)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Order_Line)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Shopping_Cart)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product_Type>()
                .HasMany(e => e.Category)
                .WithRequired(e => e.Product_Type)
                .HasForeignKey(e => e.Product_Type_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
