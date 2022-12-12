using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Orders;

namespace CoffeeShopAPI.Repositories.Orders
{
    public class SnackOrderRepository : Repository<SnackOrder>, ISnackOrderRepository
    {
        public SnackOrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<SnackOrder>();
        }
    }
}
