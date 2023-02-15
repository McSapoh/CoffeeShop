using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IIngredientService
    {
        public Task<IActionResult> Get(int id);
        public Task<IActionResult> Create(Ingredient ingredient);
        public Task<IActionResult> Update(Ingredient ingredient);
        public Task<IActionResult> Delete(int id);
    }
}
