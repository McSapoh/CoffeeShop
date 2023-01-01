using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IIngredientService
    {
        public ServiceResponse Get(int id);
        public Task<ServiceResponse> Create(Ingredient ingredient);
        public Task<ServiceResponse> Update(Ingredient ingredient);
        public Task<ServiceResponse> Delete(int id);
    }
}
