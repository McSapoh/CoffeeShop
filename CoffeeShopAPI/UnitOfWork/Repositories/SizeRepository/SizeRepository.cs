using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        public SizeRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Size>();
        }
    }
}
