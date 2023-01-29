using AutoMapper;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoffeeShopAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Products")]
    public class SandwichesController : ProductsController
    {
        public SandwichesController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger, IProductService productService, IMapper mapper) : 
            base(unitOfWork, logger, productService, mapper, ProductType.sandwich)
        { }
    }
}
