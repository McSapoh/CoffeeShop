using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Sizes;

namespace CoffeeShopAPI.Repositories.Sizes
{
    public class SandwichSizeRepository : Repository<SandwichSize>, ISandwichSizeRepository
    {
        public SandwichSizeRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<SandwichSize>();
        }
    }
}
