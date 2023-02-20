using CoffeeShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public class ConfirmEmailTokenRepository : Repository<ConfirmEmailToken>, IConfirmEmailTokenRepository
    {
        public ConfirmEmailTokenRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<ConfirmEmailToken>();
        }
        public override ConfirmEmailToken GetById(int Id) => _dbSet.FirstOrDefault(p => p.Id == Id);
        public async override Task<ConfirmEmailToken> GetByIdAsync(int Id) => await _dbSet.FirstOrDefaultAsync(p => p.Id == Id);
        public async Task<ConfirmEmailToken> GetByToken(string token) => 
            await _dbSet.SingleOrDefaultAsync(t => t.Token == token);
    }
}
