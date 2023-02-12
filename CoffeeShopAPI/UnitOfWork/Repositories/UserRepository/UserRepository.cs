using CoffeeShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<User>();
        }
        public override User GetById(int Id) => _dbSet.Include(p => p.Orders).FirstOrDefault(p => p.Id == Id);
        public async override Task<User> GetByIdAsync(int Id) => await _dbSet.Include(p => p.Orders).FirstOrDefaultAsync(p => p.Id == Id);
        public async Task<User> GetByEmail(string email) => 
            await _dbSet.Include(p => p.Orders).Include(p => p.RefreshToken)
            .FirstOrDefaultAsync(p => p.Email == email);
    }
}
