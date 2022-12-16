using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IProductService
    {
        public ServiceResponse GetProduct(int id, string Type);
        public Task<ServiceResponse> CreateProduct(Product product, IFormFile photo, string Type);
        public Task<ServiceResponse> UpdateProduct(Product product, IFormFile photo, string Type);
        public Task<ServiceResponse> DeleteProduct(int id, string Type);

    }
}
