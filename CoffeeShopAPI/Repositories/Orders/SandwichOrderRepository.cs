using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Orders;

namespace CoffeeShopAPI.Repositories.Orders
{
    public class SandwichOrderRepository : Repository<SandwichOrder>, ISandwichOrderRepository
    {
        public SandwichOrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<SandwichOrder>();
        }
    }
}
