using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Sizes;

namespace CoffeeShopAPI.Repositories.Sizes
{
    public class SnackSizeRepository : Repository<SnackSize>, ISnackSizeRepository
    {
        public SnackSizeRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<SnackSize>();
        }
    }
}
