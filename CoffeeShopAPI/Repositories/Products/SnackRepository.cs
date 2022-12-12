using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Products;

namespace CoffeeShopAPI.Repositories.Products
{
    public class SnackRepository : Repository<Snack>, ISnackRepository
    {
        public SnackRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Snack>();
        }
    }
}
