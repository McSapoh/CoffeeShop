using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetByEmail(string email);
    }
}
