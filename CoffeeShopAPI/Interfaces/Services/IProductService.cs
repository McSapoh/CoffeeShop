using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IProductService
    {
        public Task<ServiceResponse> Get(int id);
        public Task<ServiceResponse> Create(Product product, IFormFile photo);
        public Task<ServiceResponse> Update(Product product, IFormFile photo);
        public Task<ServiceResponse> Delete(int id);

    }
}
