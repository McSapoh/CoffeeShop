using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Models.Products;

namespace CoffeeShopAPI.Repositories.Products
{
    public class DessertRepository : Repository<Dessert>, IDessertRepository
    {
        public DessertRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Dessert>();
        }
    }
}
