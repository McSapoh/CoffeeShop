using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Products;

namespace CoffeeShopAPI.Repositories.Products
{
    public class CoffeeRepository : Repository<Coffee>, ICoffeeRepository
    {
        public CoffeeRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Coffee>();
        }
    }
}
