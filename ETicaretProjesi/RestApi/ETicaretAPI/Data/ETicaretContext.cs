using Microsoft.EntityFrameworkCore;
using ETicaretAPI.Models.Entities;

namespace ETicaretAPI.Data
{
    public class ETicaretContext : DbContext
    {
        public ETicaretContext(DbContextOptions<ETicaretContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ShippingCompany> ShippingCompanies { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
    }
}