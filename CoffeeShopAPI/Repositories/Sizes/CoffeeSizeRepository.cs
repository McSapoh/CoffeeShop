using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Sizes;

namespace CoffeeShopAPI.Repositories.Sizes
{
    public class CoffeeSizeRepository : Repository<CoffeeSize>, ICoffeeSizeRepository
    {
        public CoffeeSizeRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<CoffeeSize>();
        }
    }
}
