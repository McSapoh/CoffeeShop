using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Products;

namespace CoffeeShopAPI.Repositories.Products
{
    public class SandwichRepository : Repository<Sandwich>, ISandwichRepository
    {
        public SandwichRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Sandwich>();
        }
    }
}
