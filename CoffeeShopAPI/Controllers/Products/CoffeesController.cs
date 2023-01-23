using AutoMapper;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CoffeesController : ProductsController
    {
        public CoffeesController(IUnitOfWork unitOfWork, IProductService productService, IMapper mapper) : 
            base(unitOfWork, productService, mapper, ProductType.coffee)
        { }
    }
}
