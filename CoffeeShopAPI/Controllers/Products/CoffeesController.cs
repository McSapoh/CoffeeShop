using AutoMapper;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoffeeShopAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CoffeesController : ProductsController
    {
        public CoffeesController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger, IProductService productService, IMapper mapper) : 
            base(unitOfWork, logger, productService, mapper, ProductType.coffee)
        { }
    }
}
