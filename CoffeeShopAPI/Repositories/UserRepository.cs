using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<User>();
        }
        public override User GetById(int Id) => _dbSet.Include(p => p.Orders).FirstOrDefault(p => p.Id == Id);
        public async override Task<User> GetByIdAsync(int Id) => await _dbSet.Include(p => p.Orders).FirstAsync(p => p.Id == Id);
        public User GetByEmail(string email) => 
            _dbSet.Include(p => p.Orders).FirstOrDefault(p => p.Email == email);
        public PagedList<User> GetPagedList(PagingParameters pagingParameters, string type)
        {
            var items = _dbSet.Skip(
                (pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize
            ).ToList();
            return new PagedList<User>(items, _dbSet.Count(), pagingParameters.PageSize, pagingParameters.PageSize);
        }
    }
}
