using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public PagedList<Product> GetPagedList(PagingParameters pagingParameters, string type);
    }
}
