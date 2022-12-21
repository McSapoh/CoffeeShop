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
        private static async void SavePhoto(string path, IFormFile photo)
        {
            string filePath = Path.Combine(path, photo.FileName);
            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            await photo.CopyToAsync(fileStream);
        }
        private static void DeletePhoto(string path)
        {
            System.IO.File.Delete(path);
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
        public IActionResult GetProduct(int id, string type) =>
            GetResult(_productService.Get(id, type));
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int Id) =>
            GetResult(await _productService.Delete(Id));
        #endregion
        #region Coffee actions
        [HttpGet("Coffees")]
        public IActionResult GetCoffees([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.ProductRepository
                .GetPagedList(pagingParameters, ProductType.Coffee.ToString());
            var metadata = GetMetadata<Product>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateCoffee")]
        public async Task<IActionResult> CreateCoffee([FromForm] ProductDTO objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                
                var product = Product.GetByDTO(objectFromPage, ProductType.Coffee);
                return GetResult(await _productService.Create(product, photo));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateCoffee")]
        public async Task<IActionResult> UpdateCoffee([FromForm] ProductDTO objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                
                var product = Product.GetByDTO(objectFromPage, ProductType.Coffee);
                return GetResult(await _productService.Update(product, photo));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
    }
}
