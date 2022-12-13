using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
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
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        [HttpGet("GetCoffees")]
        public IActionResult GetCoffees([FromQuery] PagingParameters pagingParameters)
        {
            var coffees = _unitOfWork.CoffeeRepository.GetPagedList(pagingParameters);
            var metadata = new
            {
                coffees.TotalCount,
                coffees.PageSize,
                coffees.CurrentPage,
                coffees.TotalPages,
                coffees.HasNext,
                coffees.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(coffees);
        }
        [HttpPost("SaveCoffee")]
        public async Task<IActionResult> SaveCoffee(Coffee coffeeFromPage, IFormFile photo)
        {
            Coffee coffee;
            var IdIsNull = coffeeFromPage.Id == 0;
            if (IdIsNull)
                coffee = coffeeFromPage;
            else
            {
                coffee = _unitOfWork.CoffeeRepository.GetById(coffeeFromPage.Id);
                if (coffee == null)
                    return BadRequest(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                coffee.Name = coffeeFromPage.Name;
                coffee.Description = coffeeFromPage.Description;
                coffee.IsActive = coffeeFromPage.IsActive;
                coffee.Sizes = coffeeFromPage.Sizes;
            }
            coffee.ImagePath = "/Coffee/" + photo.FileName;

            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + "\\Images\\Coffee", photo);
                if (!IdIsNull && coffee.ImagePath != "/Coffee/DefaultCoffeeImage.png")
                    DeletePhoto(path + "\\Images" + coffee.ImagePath);
            }


            // Creating or Updating object.
            if (IdIsNull)
                _unitOfWork.CoffeeRepository.Create(coffee);
            else
                _unitOfWork.CoffeeRepository.Update(coffee);


            if (await _unitOfWork.SaveAsync())
                return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
            else
                return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
        }
        [HttpDelete("DeleteCoffee")]
        public async Task<IActionResult> DeleteCoffee(int Id)
        {
            var coffee = _unitOfWork.CoffeeRepository.GetById(Id);
            if (coffee != null)
            {
                coffee.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully deleted" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while deleting" }));
            }
            return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
        }
    }
}
