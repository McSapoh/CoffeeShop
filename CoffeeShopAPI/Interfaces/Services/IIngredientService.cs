using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IIngredientService
    {
        public ServiceResponse Get(int id, string Type);
        public Task<ServiceResponse> Create(Ingredient ingredient, string Type);
        public Task<ServiceResponse> Update(Ingredient ingredient, string Type);
        public Task<ServiceResponse> Delete(int id, string Type);
    }
}
