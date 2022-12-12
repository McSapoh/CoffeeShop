using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Products;

namespace CoffeeShopAPI.Repositories.Products
{
    public class TeaRepository : Repository<Tea>, ITeaRepository
    {
        public TeaRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Tea>();
        }
    }
}
