using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    public class IngredientsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIngredientService _ingredientService;
        public IngredientsController(IUnitOfWork unitOfWork, IIngredientService ingredientService)
        {
            _unitOfWork = unitOfWork;
            _ingredientService = ingredientService;
        }
        private IActionResult GetResult(ServiceResponse serviceResponse)
        {
            switch (serviceResponse.Status)
            {
                case 200:
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

        #region Ingredient actions.
        [HttpGet("GetIngredient")]
        public IActionResult GetIngredient(int Id) =>
            GetResult(_ingredientService.Get(Id, "Alcohol"));
        [HttpGet("GetIngredients")]
        public async Task<IActionResult> GetIngredients()
        {
            var objectsFromDb = await _unitOfWork.IngredientRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateIngredient")]
        public async Task<IActionResult> CreateIngredient(Ingredient objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.Create(objectFromPage, "Alcohol"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateIngredient")]
        public async Task<IActionResult> UpdateIngredient(Ingredient objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.Update(objectFromPage, "Alcohol"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteIngredient")]
        public async Task<IActionResult> DeleteIngredient(int Id) =>
            GetResult(await _ingredientService.Delete(Id, "Alcohol"));
        #endregion
    }
}
