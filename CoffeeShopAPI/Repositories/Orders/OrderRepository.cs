using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Orders;

namespace CoffeeShopAPI.Repositories.Orders
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Order>();
        }
    }
}
