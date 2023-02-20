using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public interface IConfirmEmailTokenRepository : IRepository<ConfirmEmailToken>
    {
        public Task<ConfirmEmailToken> GetByToken(string token);
    }
}
