using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Order>();
        }
    }
}
