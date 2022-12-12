using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Sizes;

namespace CoffeeShopAPI.Repositories.Sizes
{
    public class TeaSizeRepository : Repository<TeaSize>, ITeaSizeRepository
    {
        public TeaSizeRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<TeaSize>();
        }
    }
}
