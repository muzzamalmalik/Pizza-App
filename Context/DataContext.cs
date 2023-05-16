
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PizzaOrder.Models;
using System.Linq;
namespace PizzaOrder.Context
{
    public class DataContext : DbContext
    {


        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Deal> Deals { get; set; }
        public virtual DbSet<DealSection> DealSection { get; set; }
        public virtual DbSet<DealSectionDetail> DealSectionDetail { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemSize> ItemSize { get; set; }
        public virtual DbSet<ItemTransactionLog> ItemTransactionLog { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<PickUp> PickUp { get; set; }
        public virtual DbSet<Topping> Toppings { get; set; }
        public virtual DbSet<ToppingTransactionLog> ToppingTransactionLog { get; set; }
        public virtual DbSet<UserLoginLog> UserLoginLog { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<SlideShow> SlideShow { get; set; }
        public virtual DbSet<SlideShowImages> SlideShowImages { get; set; }
        public virtual DbSet<Crust> Crusts { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<BillPayments> BillPayments { get; set; }
        public virtual DbSet<OrderDetailAdditionalDetails> OrderDetailAdditionalDetails { get; set; }
        public virtual DbSet<OrderTransaction> OrderTransaction { get; set; }
        public virtual DbSet<OrderStatusTransaction> OrderStatusTransaction { get; set; }
        public virtual DbSet<UserDeliveryAddress> UserDeliveryAddress { get; set; }
        public virtual DbSet<FeaturedAds> FeaturedAds { get; set; }
        public virtual DbSet<ItemSizeTransection> ItemSizeTransection { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // default values

            //modelBuilder.Entity<User>()
            //    .HasIndex(u => new { u.UserName })
            //    .IsUnique(true);

            //modelBuilder.Entity<User>()
            //    .HasIndex(u => new { u.Email })
            //    .IsUnique(true);

            modelBuilder.Entity<User>()
               .HasIndex(u => new { u.ContactNumber })
               .IsUnique(true);

            modelBuilder.Entity<Company>()
               .HasIndex(u => new { u.Name })
               .IsUnique(true);
            modelBuilder.Entity<Deal>()
                           .HasIndex(u => new { u.CompanyId, u.Title })
                           .IsUnique(true);
            modelBuilder.Entity<Item>()
                           .HasIndex(u => new { u.CompanyId, u.Name })
                           .IsUnique(true);
            modelBuilder.Entity<ItemSize>()
                          .HasIndex(u => new { u.SizeDescription, u.ItemId })
                          .IsUnique(true);
            modelBuilder.Entity<DealSectionDetail>()
                          .HasIndex(u => new { u.DealSectionId, u.ItemId })
                          .IsUnique(true);
            modelBuilder.Entity<Category>()
                          .HasIndex(u => new { u.Name, u.CompanyId })
                          .IsUnique(true);
            modelBuilder.Entity<Topping>()
                           .HasIndex(u => new { u.Name,u.CompanyId, u.ItemSizeId})
                           .IsUnique(true);
            modelBuilder.Entity<Crust>()
                           .HasIndex(u => new { u.Name,u.CompanyId,u.ItemSizeId })
                           .IsUnique(true);

            // unique keys




        }
    }

}
