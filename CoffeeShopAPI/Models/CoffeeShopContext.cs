using CoffeeShopAPI.Models.Ingredients;
using CoffeeShopAPI.Models.Orders;
using CoffeeShopAPI.Models.Products;
using CoffeeShopAPI.Models.Sizes;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI.Models
{
    public class CoffeeShopContext : DbContext
    {
        public CoffeeShopContext(DbContextOptions<CoffeeShopContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }

        // Products.
        public DbSet<Coffee> Coffees { get; set; }
        public DbSet<Dessert> Desserts { get; set; }
        public DbSet<Sandwich> Sandwiches { get; set; }
        public DbSet<Snack> Snacks { get; set; }
        public DbSet<Tea> Teas { get; set; }

        // Product Sizes.
        public DbSet<CoffeeSize> CoffeeSizes { get; set; }
        public DbSet<DessertSize> DessertSizes { get; set; }
        public DbSet<SandwichSize> SandwichSizes { get; set; }
        public DbSet<SnackSize> SnackSizes { get; set; }
        public DbSet<TeaSize> TeaSizes { get; set; }

        // Ingredients.
        public DbSet<Alcohol> Alcohols { get; set; }
        public DbSet<Milk> Milks { get; set; }
        public DbSet<Sauce> Sauces { get; set; }
        public DbSet<Supplements> Supplements { get; set; }
        public DbSet<Syrup> Syrups { get; set; }

        // Orders.
        public DbSet<Order> Orders { get; set; }
        public DbSet<CoffeeOrder> CoffeeOrders { get; set; }
        public DbSet<DessertOrder> DessertOrders { get; set; }
        public DbSet<SandwichOrder> SandwichOrders { get; set; }
        public DbSet<SnackOrder> SnackOrders { get; set; }
        public DbSet<TeaOrder> TeaOrders { get; set; }

    }
}
