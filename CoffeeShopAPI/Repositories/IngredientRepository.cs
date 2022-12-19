using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Repositories
{
    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Ingredient>();
        }
    }
}
