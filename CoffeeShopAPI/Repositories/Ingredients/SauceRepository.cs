using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Ingredients;

namespace CoffeeShopAPI.Repositories.Ingredients
{
    public class SauceRepository : Repository<Sauce>, ISauceRepository
    {
        public SauceRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Sauce>();
        }
    }
}
