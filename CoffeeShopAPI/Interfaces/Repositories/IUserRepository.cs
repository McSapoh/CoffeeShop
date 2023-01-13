using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetByEmail(string email);
    }
}
