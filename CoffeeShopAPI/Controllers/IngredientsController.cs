using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIngredient(int Id) => GetResult(await _ingredientService.Get(Id));
        [HttpPut("UpdateIngredient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateIngredient(IngredientDTO objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));

                var ingredient = Ingredient.GetByDTO(objectFromPage, IngredientType.alcohol);
                return GetResult(await _ingredientService.Update(ingredient));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteIngredient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredient(int Id) =>
            GetResult(await _ingredientService.Delete(Id));
        #endregion
    }
}
