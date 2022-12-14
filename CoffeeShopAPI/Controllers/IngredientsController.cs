using AutoMapper;
using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    public class IngredientsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIngredientService _ingredientService;
        private readonly IMapper _mapper;

        public IngredientsController(IUnitOfWork unitOfWork, IIngredientService ingredientService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _ingredientService = ingredientService;
            _mapper = mapper;
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

                var ingredient = _mapper.Map<Ingredient>(objectFromPage);
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
        #region Alcohol actions
        [HttpGet("Alcohols")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAlcohols([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.IngredientRepository
                .GetPagedList(pagingParameters, IngredientType.alcohol.ToString());
            var metadata = GetMetadata<Ingredient>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateAlcohol")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAlcohol([FromForm] IngredientDTO objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));

                var ingredient = _mapper.Map<Ingredient>(objectFromPage);
                ingredient.IngredientType = IngredientType.alcohol.ToString();
                return GetResult(await _ingredientService.Create(ingredient));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
        #region Milk actions
        [HttpGet("Milks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMilks([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.IngredientRepository
                .GetPagedList(pagingParameters, IngredientType.milk.ToString());
            var metadata = GetMetadata<Ingredient>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateMilk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMilk([FromForm] IngredientDTO objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));

                var ingredient = _mapper.Map<Ingredient>(objectFromPage);
                ingredient.IngredientType = IngredientType.milk.ToString();
                return GetResult(await _ingredientService.Create(ingredient));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
        #region Sauce actions
        [HttpGet("Sauces")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetSauces([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.IngredientRepository
                .GetPagedList(pagingParameters, IngredientType.sauce.ToString());
            var metadata = GetMetadata<Ingredient>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSauce")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSauce([FromForm] IngredientDTO objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));

                var ingredient = _mapper.Map<Ingredient>(objectFromPage);
                ingredient.IngredientType = IngredientType.sauce.ToString();
                return GetResult(await _ingredientService.Create(ingredient));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
        #region Supplements actions
        [HttpGet("Supplements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetSupplements([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.IngredientRepository
                .GetPagedList(pagingParameters, IngredientType.supplements.ToString());
            var metadata = GetMetadata<Ingredient>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSupplements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSupplements([FromForm] IngredientDTO objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));

                var ingredient = _mapper.Map<Ingredient>(objectFromPage);
                ingredient.IngredientType = IngredientType.supplements.ToString();
                return GetResult(await _ingredientService.Create(ingredient));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
        #region Syrup actions
        [HttpGet("Syrups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetSyrups([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.IngredientRepository
                .GetPagedList(pagingParameters, IngredientType.syrup.ToString());
            var metadata = GetMetadata<Ingredient>(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSyrups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSyrups([FromForm] IngredientDTO objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));

                var ingredient = _mapper.Map<Ingredient>(objectFromPage);
                ingredient.IngredientType = IngredientType.syrup.ToString();
                return GetResult(await _ingredientService.Create(ingredient));
            }
            else
                return BadRequest(ModelState);
        }
        #endregion
    }
}
