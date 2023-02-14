using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IProductService
    {
        public Task<IActionResult> Get(int id);
        public Task<IActionResult> Create(Product product, IFormFile photo);
        public Task<IActionResult> Update(Product product, IFormFile photo);
        public Task<IActionResult> Delete(int id);

    }
}
