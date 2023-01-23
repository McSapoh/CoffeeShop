using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Product>();
        }
        public override Product GetById(int Id) => _dbSet.Include(p => p.Sizes).FirstOrDefault(p => p.Id == Id);
        public async override Task<Product> GetByIdAsync(int Id) => await _dbSet.Include(p => p.Sizes).FirstOrDefaultAsync(p => p.Id == Id);
        public PagedList<Product> GetPagedList(PagingParameters pagingParameters, string type)
        {
            var items = _dbSet.Include(p => p.Sizes.Where(s => s.IsActive == true)).Where(o => o.ProductType == type).Skip(
                (pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize
            ).ToList();
            return new PagedList<Product>(items, _dbSet.Count(), pagingParameters.PageSize, pagingParameters.PageSize);
        }
    }
}
