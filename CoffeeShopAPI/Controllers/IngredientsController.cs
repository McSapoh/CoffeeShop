using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models.Ingredients;
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

        #region Alkohol actions.
        [HttpGet("GetAlcohol")]
        public IActionResult GetAlcohol(int Id) =>
            GetResult(_ingredientService.GetIngredient(Id, "Alcohol"));
        [HttpGet("GetAllAlcohol")]
        public async Task<IActionResult> GetAllAlcohol()
        {
            var objectsFromDb = await _unitOfWork.AlcoholRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateAlcohol")]
        public async Task<IActionResult> CreateAlcohol(Alcohol objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.CreateIngredient(objectFromPage, "Alcohol"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateAlcohol")]
        public async Task<IActionResult> UpdateAlcohol(Alcohol objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.UpdateIngredient(objectFromPage, "Alcohol"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteAlcohol")]
        public async Task<IActionResult> DeleteAlcohol(int Id) =>
            GetResult(await _ingredientService.DeleteIngredient(Id, "Alcohol"));
        #endregion
        #region Milk actions.
        [HttpGet("GetMilk")]
        public IActionResult GetMilk(int Id) =>
            GetResult(_ingredientService.GetIngredient(Id, "Milk"));
        [HttpGet("GetAllMilk")]
        public async Task<IActionResult> GetAllMilk()
        {
            var objectsFromDb = await _unitOfWork.MilkRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateMilk")]
        public async Task<IActionResult> CreateMilk(Milk objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.CreateIngredient(objectFromPage, "Milk"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateMilk")]
        public async Task<IActionResult> UpdateMilk(Milk objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.UpdateIngredient(objectFromPage, "Milk"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteMilk")]
        public async Task<IActionResult> DeleteMilk(int Id) =>
            GetResult(await _ingredientService.DeleteIngredient(Id, "Milk"));
        #endregion
        #region Sauce actions.
        [HttpGet("GetSauce")]
        public IActionResult GetSauce(int Id) =>
            GetResult(_ingredientService.GetIngredient(Id, "Sauce"));
        [HttpGet("GetAllSauce")]
        public async Task<IActionResult> GetAllSauce()
        {
            var objectsFromDb = await _unitOfWork.SauceRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSauce")]
        public async Task<IActionResult> CreateSauce(Sauce objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.CreateIngredient(objectFromPage, "Sauce"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateSauce")]
        public async Task<IActionResult> UpdateSauce(Sauce objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.UpdateIngredient(objectFromPage, "Sauce"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSauce")]
        public async Task<IActionResult> DeleteSauce(int Id) =>
            GetResult(await _ingredientService.DeleteIngredient(Id, "Milk"));
        #endregion
        #region Supplements actions.
        [HttpGet("GetSupplements")]
        public IActionResult GetSupplements(int Id) =>
            GetResult(_ingredientService.GetIngredient(Id, "Supplements"));
        [HttpGet("GetAllSupplements")]
        public async Task<IActionResult> GetAllSupplements()
        {
            var objectsFromDb = await _unitOfWork.SupplementsRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSupplements")]
        public async Task<IActionResult> CreateSupplements(Supplements objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.CreateIngredient(objectFromPage, "Supplements"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateSupplements")]
        public async Task<IActionResult> UpdateSupplements(Supplements objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.UpdateIngredient(objectFromPage, "Supplements"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSupplements")]
        public async Task<IActionResult> DeleteSupplements(int Id) =>
            GetResult(await _ingredientService.DeleteIngredient(Id, "Supplemets"));
        #endregion
        #region Syrup actions.
        [HttpGet("GetSyrup")]
        public IActionResult GetSyrup(int Id) =>
            GetResult(_ingredientService.GetIngredient(Id, "Syrup"));
        [HttpGet("GetAllSyrup")]
        public async Task<IActionResult> GetAllSyrup()
        {
            var objectsFromDb = await _unitOfWork.SyrupRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("CreateSyrup")]
        public async Task<IActionResult> CreateSyrup(Syrup objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id != 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot create object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.CreateIngredient(objectFromPage, "Syrup"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("UpdateSyrup")]
        public async Task<IActionResult> UpdateSyrup(Syrup objectFromPage)
        {
            if (ModelState.IsValid)
            {
                if (objectFromPage.Id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {objectFromPage.Id}" }));
                return GetResult(await _ingredientService.UpdateIngredient(objectFromPage, "Syrup"));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSyrup")]
        public async Task<IActionResult> DeleteSyrup(int Id) =>
            GetResult(await _ingredientService.DeleteIngredient(Id, "Syrup"));
        #endregion
    }
}
