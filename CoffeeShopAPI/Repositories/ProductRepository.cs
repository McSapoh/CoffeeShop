using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Product>();
        }
    }
}
