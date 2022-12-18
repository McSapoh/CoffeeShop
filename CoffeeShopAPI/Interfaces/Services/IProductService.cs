using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IProductService
    {
        public ServiceResponse Get(int id, string Type);
        public Task<ServiceResponse> Create(Product product, IFormFile photo, string Type);
        public Task<ServiceResponse> Update(Product product, IFormFile photo, string Type);
        public Task<ServiceResponse> Delete(int id, string Type);

    }
}
