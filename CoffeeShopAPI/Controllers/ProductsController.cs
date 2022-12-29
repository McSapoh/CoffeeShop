using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{

    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public ProductsController(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }
        private IActionResult GetResult (ServiceResponse serviceResponse)
        {
            switch (serviceResponse.Status)
            {
                case 200 :
                {
                    if (serviceResponse.Data != null)
                        return Ok(new JsonResult(new
                        {
                            data = serviceResponse.Data
                        }));
                    return Ok(new JsonResult(new
                    {
                        success = serviceResponse.Success,
                        message = serviceResponse.Message
                    }));
                }
                case 400:
                {
                    return BadRequest(new JsonResult(new
                    {
                        success = serviceResponse.Success,
                        message = serviceResponse.Message
                    }));
                }
                case 404:
                {
                    return NotFound(new JsonResult(new
                    {
                        success = serviceResponse.Success,
                        message = serviceResponse.Message
                    }));
                }
                default:
                    return BadRequest(new JsonResult(new { success = false, message = "Unknown result" }));
            }
        }
        public dynamic GetMetadata<T>(PagedList<T> objects)
        {
            var metadata = new
            {
                objects.TotalCount,
                objects.PageSize,
                objects.CurrentPage,
                objects.TotalPages,
                objects.HasNext,
                objects.HasPrevious
            };
            return metadata;
        }
        #region Product actions.
        [HttpGet("GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id) => GetResult(await _productService.Get(id));

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDTO objectFromPage, IFormFile photo)
        {
            if (photo != null && !photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));

                var product = Product.GetByDTO(objectFromPage, ProductType.coffee);
                return GetResult(await _productService.Update(product, photo));
            }
            else
                return BadRequest(ModelState);
        }

        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int Id) =>
            GetResult(await _productService.Delete(Id));
        #endregion
        #region Coffee actions
        [HttpGet("Coffees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCoffees([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.ProductRepository
                .GetPagedList(pagingParameters, ProductType.coffee.ToString());
            var metadata = GetMetadata<Product>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateCoffee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCoffee([FromForm] ProductDTO objectFromPage, IFormFile photo)
        {
            if (photo != null && !photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                
                var product = Product.GetByDTO(objectFromPage, ProductType.coffee);
                return GetResult(await _productService.Create(product, photo));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
        #region Desserts actions
        [HttpGet("Desserts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetDesserts([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.ProductRepository
                .GetPagedList(pagingParameters, ProductType.dessert.ToString());
            var metadata = GetMetadata<Product>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateDessert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDessert([FromForm] ProductDTO objectFromPage, IFormFile photo)
        {
            if (photo != null && !photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));

                var product = Product.GetByDTO(objectFromPage, ProductType.dessert);
                return GetResult(await _productService.Create(product, photo));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
    }
}
