using CoffeeShopAPI.Helpers.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        public T GetById(int Id);
        public Task<IEnumerable<T>> GetAll();
        public PagedList<T> GetPagedList(PagingParameters pagingParameters);
        public void Create(T Item);
        public void Update(T Item);
        public void Delete(int Id);
        public void Delete(T Item);
    }
}

