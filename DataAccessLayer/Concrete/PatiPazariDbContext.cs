using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class PatiPazariDbContext : DbContext
    {
        public PatiPazariDbContext(DbContextOptions<PatiPazariDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Baskets { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.Advert)
        //        .WithMany()
        //        .HasForeignKey(o => o.AdvertID)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.User)
        //        .WithMany(u => u.Orders)
        //        .HasForeignKey(o => o.UserID)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Advert>()
        //        .HasOne(a => a.User)
        //        .WithMany(u => u.Adverts)
        //        .HasForeignKey(a => a.UserID)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Advert>()
        //        .HasOne(a => a.Category)
        //        .WithMany(c => c.Adverts)
        //        .HasForeignKey(a => a.CategoryID)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}
    }
}
