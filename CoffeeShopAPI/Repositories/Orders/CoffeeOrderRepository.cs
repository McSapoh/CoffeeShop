using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Orders;

namespace CoffeeShopAPI.Repositories.Orders
{
    public class CoffeeOrderRepository : Repository<CoffeeOrder>, ICoffeeOrderRepository
    {
        public CoffeeOrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<CoffeeOrder>();
        }
    }
}
