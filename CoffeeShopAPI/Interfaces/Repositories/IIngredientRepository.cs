using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        public PagedList<Ingredient> GetPagedList(PagingParameters pagingParameters, string type);
    }
}
