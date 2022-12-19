using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Order>();
        }
    }
}
