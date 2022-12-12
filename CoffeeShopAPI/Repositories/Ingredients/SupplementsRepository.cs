using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Ingredients;

namespace CoffeeShopAPI.Repositories.Ingredients
{
    public class SupplementsRepository : Repository<Supplements>, ISupplementsRepository
    {
        public SupplementsRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Supplements>();
        }
    }
}
