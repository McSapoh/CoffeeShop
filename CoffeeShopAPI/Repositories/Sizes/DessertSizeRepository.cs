using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Sizes;

namespace CoffeeShopAPI.Repositories.Sizes
{
    public class DessertSizeRepository : Repository<DessertSize>, IDessertSizeRepository
    {
        public DessertSizeRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<DessertSize>();
        }
    }
}
