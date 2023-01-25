using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        public PagedList<Ingredient> GetPagedList(PagingParameters pagingParameters, string type);
    }
}
