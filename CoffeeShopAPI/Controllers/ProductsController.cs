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


        #region Coffee actions.
        [HttpGet("GetCoffee")]
        public IActionResult GetCoffee(int Id)
        {
            var objectFromDb = _unitOfWork.CoffeeRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetCoffees")]
        public IActionResult GetCoffees([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.CoffeeRepository.GetPagedList(pagingParameters);
            var metadata = new
            {
                objectsFromDb.TotalCount,
                objectsFromDb.PageSize,
                objectsFromDb.CurrentPage,
                objectsFromDb.TotalPages,
                objectsFromDb.HasNext,
                objectsFromDb.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveCoffee")]
        public async Task<IActionResult> SaveCoffee(Coffee objectFromPage, IFormFile photo)
        {
            Coffee objectFromDb;
            var IdIsNull = objectFromPage.Id == 0;
            if (IdIsNull)
                objectFromDb = objectFromPage;
            else
            {
                objectFromDb = _unitOfWork.CoffeeRepository.GetById(objectFromPage.Id);
                if (objectFromDb == null)
                    return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                objectFromDb.Name = objectFromPage.Name;
                objectFromDb.Description = objectFromPage.Description;
                objectFromDb.IsActive = objectFromPage.IsActive;
                objectFromDb.Sizes = objectFromPage.Sizes;
                objectFromDb.ImagePath = "/Coffee/" + photo.FileName;
            }

            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + "\\Images\\Coffee", photo);
                if (!IdIsNull && objectFromDb.ImagePath != "/Coffee/DefaultCoffeeImage.png")
                    DeletePhoto(path + "\\Images" + objectFromDb.ImagePath);
            }


            // Creating or Updating object.
            if (IdIsNull)
                _unitOfWork.CoffeeRepository.Create(objectFromDb);
            else
                _unitOfWork.CoffeeRepository.Update(objectFromDb);


            if (await _unitOfWork.SaveAsync())
                return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
            else
                return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
        }
        [HttpDelete("DeleteCoffee")]
        public async Task<IActionResult> DeleteCoffee(int Id)
        {
            var objectFromDb = _unitOfWork.CoffeeRepository.GetById(Id);
            if (objectFromDb != null)
            {
                if (!objectFromDb.IsActive)
                    return BadRequest(new JsonResult(new { success = false, message = "Cannot delete already deleted object" }));
                objectFromDb.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully deleted" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while deleting" }));
            }
            return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
        }
        #endregion
        #region Dessert actions.
        [HttpGet("GetDessert")]
        public IActionResult GetDessert(int Id)
        {
            var objectFromDb = _unitOfWork.DessertRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetDesserts")]
        public IActionResult GetDesserts([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.DessertRepository.GetPagedList(pagingParameters);
            var metadata = new
            {
                objectsFromDb.TotalCount,
                objectsFromDb.PageSize,
                objectsFromDb.CurrentPage,
                objectsFromDb.TotalPages,
                objectsFromDb.HasNext,
                objectsFromDb.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveDessert")]
        public async Task<IActionResult> SaveDessert(Dessert objectFromPage, IFormFile photo)
        {
            Dessert objectFromDb;
            var IdIsNull = objectFromPage.Id == 0;
            if (IdIsNull)
                objectFromDb = objectFromPage;
            else
            {
                objectFromDb = _unitOfWork.DessertRepository.GetById(objectFromPage.Id);
                if (objectFromDb == null)
                    return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                objectFromDb.Name = objectFromPage.Name;
                objectFromDb.Description = objectFromPage.Description;
                objectFromDb.IsActive = objectFromPage.IsActive;
                objectFromDb.Sizes = objectFromPage.Sizes;
                objectFromDb.ImagePath = "/Dessert/" + photo.FileName;
            }

            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + "\\Images\\Dessert", photo);
                if (!IdIsNull && objectFromDb.ImagePath != "/Dessert/DefaultDessertImage.png")
                    DeletePhoto(path + "\\Images" + objectFromDb.ImagePath);
            }


            // Creating or Updating object.
            if (IdIsNull)
                _unitOfWork.DessertRepository.Create(objectFromDb);
            else
                _unitOfWork.DessertRepository.Update(objectFromDb);


            if (await _unitOfWork.SaveAsync())
                return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
            else
                return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
        }
        [HttpDelete("DeleteDessert")]
        public async Task<IActionResult> DeleteDessert(int Id)
        {
            var objectFromDb = _unitOfWork.DessertRepository.GetById(Id);
            if (objectFromDb != null)
            {
                if (!objectFromDb.IsActive)
                    return BadRequest(new JsonResult(new { success = false, message = "Cannot delete already deleted object" }));
                objectFromDb.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully deleted" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while deleting" }));
            }
            return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
        }
        #endregion
        #region Sandwich actions.
        [HttpGet("GetSandwich")]
        public IActionResult GetSandwich(int Id)
        {
            var objectFromDb = _unitOfWork.SandwichRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetSandwiches")]
        public IActionResult GetSandwiches([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.SandwichRepository.GetPagedList(pagingParameters);
            var metadata = new
            {
                objectsFromDb.TotalCount,
                objectsFromDb.PageSize,
                objectsFromDb.CurrentPage,
                objectsFromDb.TotalPages,
                objectsFromDb.HasNext,
                objectsFromDb.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveSandwich")]
        public async Task<IActionResult> SaveSandwich(Sandwich objectFromPage, IFormFile photo)
        {
            Sandwich objectFromDb;
            var IdIsNull = objectFromPage.Id == 0;
            if (IdIsNull)
                objectFromDb = objectFromPage;
            else
            {
                objectFromDb = _unitOfWork.SandwichRepository.GetById(objectFromPage.Id);
                if (objectFromDb == null)
                    return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                objectFromDb.Name = objectFromPage.Name;
                objectFromDb.Description = objectFromPage.Description;
                objectFromDb.IsActive = objectFromPage.IsActive;
                objectFromDb.Sizes = objectFromPage.Sizes;
                objectFromDb.ImagePath = "/Sandwich/" + photo.FileName;
            }

            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + "\\Images\\Sandwich", photo);
                if (!IdIsNull && objectFromDb.ImagePath != "/Sandwich/DefaultSandwichImage.png")
                    DeletePhoto(path + "\\Images" + objectFromDb.ImagePath);
            }


            // Creating or Updating object.
            if (IdIsNull)
                _unitOfWork.SandwichRepository.Create(objectFromDb);
            else
                _unitOfWork.SandwichRepository.Update(objectFromDb);


            if (await _unitOfWork.SaveAsync())
                return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
            else
                return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
        }
        [HttpDelete("DeleteSandwich")]
        public async Task<IActionResult> DeleteSandwich(int Id)
        {
            var objectFromDb = _unitOfWork.SandwichRepository.GetById(Id);
            if (objectFromDb != null)
            {
                if (!objectFromDb.IsActive)
                    return BadRequest(new JsonResult(new { success = false, message = "Cannot delete already deleted object" }));
                objectFromDb.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully deleted" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while deleting" }));
            }
            return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
        }
        #endregion
        #region Snack actions.
        [HttpGet("GetSnack")]
        public IActionResult GetSnack(int Id)
        {
            var objectFromDb = _unitOfWork.SnackRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetSnacks")]
        public IActionResult GetSnacks([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.SnackRepository.GetPagedList(pagingParameters);
            var metadata = new
            {
                objectsFromDb.TotalCount,
                objectsFromDb.PageSize,
                objectsFromDb.CurrentPage,
                objectsFromDb.TotalPages,
                objectsFromDb.HasNext,
                objectsFromDb.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveSnack")]
        public async Task<IActionResult> SaveSnack(Snack objectFromPage, IFormFile photo)
        {
            Snack objectFromDb;
            var IdIsNull = objectFromPage.Id == 0;
            if (IdIsNull)
                objectFromDb = objectFromPage;
            else
            {
                objectFromDb = _unitOfWork.SnackRepository.GetById(objectFromPage.Id);
                if (objectFromDb == null)
                    return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                objectFromDb.Name = objectFromPage.Name;
                objectFromDb.Description = objectFromPage.Description;
                objectFromDb.IsActive = objectFromPage.IsActive;
                objectFromDb.Sizes = objectFromPage.Sizes;
                objectFromDb.ImagePath = "/Dessert/" + photo.FileName;
            }

            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + "\\Images\\Dessert", photo);
                if (!IdIsNull && objectFromDb.ImagePath != "/Dessert/DefaultDessertImage.png")
                    DeletePhoto(path + "\\Images" + objectFromDb.ImagePath);
            }


            // Creating or Updating object.
            if (IdIsNull)
                _unitOfWork.SnackRepository.Create(objectFromDb);
            else
                _unitOfWork.SnackRepository.Update(objectFromDb);


            if (await _unitOfWork.SaveAsync())
                return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
            else
                return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
        }
        [HttpDelete("DeleteSnack")]
        public async Task<IActionResult> DeleteSnack(int Id)
        {
            var objectFromDb = _unitOfWork.SnackRepository.GetById(Id);
            if (objectFromDb != null)
            {
                if (!objectFromDb.IsActive)
                    return BadRequest(new JsonResult(new { success = false, message = "Cannot delete already deleted object" }));
                objectFromDb.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully deleted" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while deleting" }));
            }
            return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
        }
        #endregion
        #region Tea actions.
        [HttpGet("GetTea")]
        public IActionResult GetTea(int Id)
        {
            var objectFromDb = _unitOfWork.TeaRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetTeas")]
        public IActionResult GetTea([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.TeaRepository.GetPagedList(pagingParameters);
            var metadata = new
            {
                objectsFromDb.TotalCount,
                objectsFromDb.PageSize,
                objectsFromDb.CurrentPage,
                objectsFromDb.TotalPages,
                objectsFromDb.HasNext,
                objectsFromDb.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveTea")]
        public async Task<IActionResult> SaveTea(Tea objectFromPage, IFormFile photo)
        {
            Tea objectFromDb;
            var IdIsNull = objectFromPage.Id == 0;
            if (IdIsNull)
                objectFromDb = objectFromPage;
            else
            {
                objectFromDb = _unitOfWork.TeaRepository.GetById(objectFromPage.Id);
                if (objectFromDb == null)
                    return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                objectFromDb.Name = objectFromPage.Name;
                objectFromDb.Description = objectFromPage.Description;
                objectFromDb.IsActive = objectFromPage.IsActive;
                objectFromDb.Sizes = objectFromPage.Sizes;
                objectFromDb.ImagePath = "/Sandwich/" + photo.FileName;
            }

            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + "\\Images\\Tea", photo);
                if (!IdIsNull && objectFromDb.ImagePath != "/Sandwich/DefaultTeaImage.png")
                    DeletePhoto(path + "\\Images" + objectFromDb.ImagePath);
            }


            // Creating or Updating object.
            if (IdIsNull)
                _unitOfWork.TeaRepository.Create(objectFromDb);
            else
                _unitOfWork.TeaRepository.Update(objectFromDb);


            if (await _unitOfWork.SaveAsync())
                return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
            else
                return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
        }
        [HttpDelete("DeleteTea")]
        public async Task<IActionResult> DeleteTea(int Id)
        {
            var objectFromDb = _unitOfWork.TeaRepository.GetById(Id);
            if (objectFromDb != null)
            {
                if (!objectFromDb.IsActive)
                    return BadRequest(new JsonResult(new { success = false, message = "Cannot delete already deleted object" }));
                objectFromDb.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully deleted" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while deleting" }));
            }
            return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
        }
        #endregion
    }
}
