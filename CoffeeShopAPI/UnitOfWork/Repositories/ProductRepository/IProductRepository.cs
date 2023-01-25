using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public PagedList<Product> GetPagedList(PagingParameters pagingParameters, string type);
    }
}
