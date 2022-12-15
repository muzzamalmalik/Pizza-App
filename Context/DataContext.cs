
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
        public virtual DbSet<BillPayments> BillPayments { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // default values

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.UserName })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Email })
                .IsUnique(true);

            modelBuilder.Entity<User>()
               .HasIndex(u => new { u.ContactNumber })
               .IsUnique(true);

            // unique keys




        }
    }

}
