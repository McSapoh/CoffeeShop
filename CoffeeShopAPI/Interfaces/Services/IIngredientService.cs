using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.Services;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IIngredientService
    {
        public ServiceResponse GetIngredient(int id, string Type);
        public Task<ServiceResponse> CreateIngredient(Ingredient ingredient, string Type);
        public Task<ServiceResponse> UpdateIngredient(Ingredient ingredient, string Type);
        public Task<ServiceResponse> DeleteIngredient(int id, string Type);
    }
}
