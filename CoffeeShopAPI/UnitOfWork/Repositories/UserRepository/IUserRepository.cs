using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetByEmail(string email);
        public Task<User> GetByRefreshToken(string refreshToken);
    }
}
