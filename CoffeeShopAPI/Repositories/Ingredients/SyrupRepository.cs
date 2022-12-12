using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Ingredients;

namespace CoffeeShopAPI.Repositories.Ingredients
{
    public class SyrupRepository : Repository<Syrup>, ISyrupRepository
    {
        public SyrupRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Syrup>();
        }
    }
}
