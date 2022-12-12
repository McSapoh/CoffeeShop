using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Ingredients;

namespace CoffeeShopAPI.Repositories.Ingredients
{
    public class AlcoholRepository : Repository<Alcohol>, IAlcoholRepository
    {
        public AlcoholRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Alcohol>();
        }
    }
}
