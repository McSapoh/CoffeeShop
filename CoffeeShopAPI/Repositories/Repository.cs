using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T: class
    {
        protected DbSet<T> _dbSet;
        public void Create(T Item) => _dbSet.Add(Item);
        public void Update(T Item) => _dbSet.Update(Item);
        public void Delete(int Id) => _dbSet.Remove(GetById(Id));
        public void Delete(T Item) => _dbSet.Remove(Item);
        public async Task<IEnumerable<T>> GetAll() => (await _dbSet.ToListAsync()).AsEnumerable();
        public PagedList<T> GetPagedList(PagingParameters pagingParameters)
        {
            var items = _dbSet.Skip(
                (pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize
            ).ToList();
            return new PagedList<T>(items, _dbSet.Count(), pagingParameters.PageSize, pagingParameters.PageSize);
        }
        public virtual T GetById(int Id) => _dbSet.Find(Id);
        public virtual async Task<T> GetByIdAsync(int Id) => await _dbSet.FindAsync(Id);

    }
}
