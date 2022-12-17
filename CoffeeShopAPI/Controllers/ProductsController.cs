using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Helpers.Services;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
        #region Coffee actions.
        [HttpGet("GetCoffee")]
        public IActionResult GetCoffee(int Id) =>
            GetResult(_productService.GetProduct(Id, "Coffee"));
        [HttpGet("GetCoffees")]
        public IActionResult GetCoffees([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.CoffeeRepository.GetPagedList(pagingParameters);
            var metadata = GetMetadata<Coffee>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateCoffee")]
        public async Task<IActionResult> CreateCoffee(Coffee objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.CreateProduct(objectFromPage, photo, "Coffee"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateCoffee")]
        public async Task<IActionResult> UpdateCoffee(Coffee objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.UpdateProduct(objectFromPage, photo, "Coffee"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCoffee")]
        public async Task<IActionResult> DeleteCoffee(int Id) =>
            GetResult(await _productService.DeleteProduct(Id, "Coffee"));
        #endregion
        #region Dessert actions.
        [HttpGet("GetDessert")]
        public IActionResult GetDessert(int Id) =>
            GetResult(_productService.GetProduct(Id, "Dessert"));
        [HttpGet("GetDesserts")]
        public IActionResult GetDesserts([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.DessertRepository.GetPagedList(pagingParameters);
            var metadata = GetMetadata<Dessert>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateDessert")]
        public async Task<IActionResult> CreateDessert(Dessert objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.CreateProduct(objectFromPage, photo, "Dessert"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateDessert")]
        public async Task<IActionResult> UpdateDessert(Dessert objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.UpdateProduct(objectFromPage, photo, "Dessert"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteDessert")]
        public async Task<IActionResult> DeleteDessert(int Id) =>
            GetResult(await _productService.DeleteProduct(Id, "Dessert"));
        #endregion
        #region Sandwich actions.
        [HttpGet("GetSandwich")]
        public IActionResult GetSandwich(int Id) =>
            GetResult(_productService.GetProduct(Id, "Sandwich"));
        [HttpGet("GetSandwiches")]
        public IActionResult GetSandwiches([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.SandwichRepository.GetPagedList(pagingParameters);
            var metadata = GetMetadata<Sandwich>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSandwich")]
        public async Task<IActionResult> CreateSandwich(Sandwich objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.CreateProduct(objectFromPage, photo, "Sandwich"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateSandwich")]
        public async Task<IActionResult> UpdateSandwich(Sandwich objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.UpdateProduct(objectFromPage, photo, "Sandwich"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSandwich")]
        public async Task<IActionResult> DeleteSandwich(int Id) =>
            GetResult(await _productService.DeleteProduct(Id, "Sandwich"));
        #endregion
        #region Snack actions.
        [HttpGet("GetSnack")]
        public IActionResult GetSnack(int Id) =>
            GetResult(_productService.GetProduct(Id, "Snack"));
        [HttpGet("GetSnacks")]
        public IActionResult GetSnacks([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.SnackRepository.GetPagedList(pagingParameters);
            var metadata = GetMetadata<Snack>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSnack")]
        public async Task<IActionResult> CreateSnack(Snack objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.CreateProduct(objectFromPage, photo, "Snack"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateSnack")]
        public async Task<IActionResult> UpdateSnack(Snack objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.UpdateProduct(objectFromPage, photo, "Snack"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSnack")]
        public async Task<IActionResult> DeleteSnack(int Id) =>
            GetResult(await _productService.DeleteProduct(Id, "Snack"));
        #endregion
        #region Tea actions.
        [HttpGet("GetTea")]
        public IActionResult GetTea(int Id) =>
            GetResult(_productService.GetProduct(Id, "Tea"));
        [HttpGet("GetTeas")]
        public IActionResult GetTea([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.TeaRepository.GetPagedList(pagingParameters);
            var metadata = GetMetadata<Tea>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateTea")]
        public async Task<IActionResult> CreateTea(Tea objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.CreateProduct(objectFromPage, photo, "Tea"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateTea")]
        public async Task<IActionResult> UpdateTea(Tea objectFromPage, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _productService.UpdateProduct(objectFromPage, photo, "Tea"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteTea")]
        public async Task<IActionResult> DeleteTea(int Id) => 
            GetResult(await _productService.DeleteProduct(Id, "Tea"));
        #endregion
    }
}
