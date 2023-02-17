using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI.Models
{
    public class CoffeeShopContext : DbContext
    {
        public CoffeeShopContext(DbContextOptions<CoffeeShopContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; set; }
    }
}
