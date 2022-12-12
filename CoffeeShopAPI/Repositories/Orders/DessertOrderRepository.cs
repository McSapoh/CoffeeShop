using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Orders;

namespace CoffeeShopAPI.Repositories.Orders
{
    public class DessertOrderRepository : Repository<DessertOrder>, IDessertOrderRepository
    {
        public DessertOrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<DessertOrder>();
        }
    }
}
