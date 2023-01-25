using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public class ProductOrderRepository : Repository<ProductOrder>, IProductOrderRepository
    {
        public ProductOrderRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<ProductOrder>();
        }
    }
}
