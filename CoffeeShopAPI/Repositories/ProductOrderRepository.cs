using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Repositories
{
    public class ProductOrderRepository : Repository<ProductOrder>, IProductOrderRepository
    {
        public ProductOrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<ProductOrder>();
        }
    }
}
