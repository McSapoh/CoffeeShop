using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Ingredients;

namespace CoffeeShopAPI.Repositories.Ingredients
{
    public class MilkRepository : Repository<Milk>, IMilkRepository
    {
        public MilkRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Milk>();
        }
    }
}
