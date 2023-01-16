using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetByEmail(string email);
        public PagedList<User> GetPagedList(PagingParameters pagingParameters, string type);
    }
}
