using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<User>();
        }
    }
}
