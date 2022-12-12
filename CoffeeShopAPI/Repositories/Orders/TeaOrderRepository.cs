using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Orders;

namespace CoffeeShopAPI.Repositories.Orders
{
    public class TeaOrderRepository : Repository<TeaOrder>, ITeaOrderRepository
    {
        public TeaOrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<TeaOrder>();
        }
    }
}
