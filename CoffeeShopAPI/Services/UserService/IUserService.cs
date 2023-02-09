using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IUserService
    {
        public Task<IActionResult> Create(User user, IFormFile photo);
        public Task<IActionResult> Update(User user, IFormFile photo);
    }
}
