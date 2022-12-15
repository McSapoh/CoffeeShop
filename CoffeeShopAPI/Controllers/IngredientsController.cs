using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models.Ingredients;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    public class IngredientsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IngredientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Alkohol actions.
        [HttpGet("GetAlcohol")]
        public IActionResult GetAlcohol(int Id)
        {
            var objectFromDb = _unitOfWork.AlcoholRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetAllAlcohol")]
        public async Task<IActionResult> GetAllAlcohol()
        {
            var objectsFromDb = await _unitOfWork.AlcoholRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveAlcohol")]
        public async Task<IActionResult> SaveAlcohol(Alcohol objectFromPage)
        {
            if (ModelState.IsValid)
            {
                Alcohol objectFromDb;
                var IdIsNull = objectFromPage.Id == 0;
                if (IdIsNull)
                    objectFromDb = objectFromPage;
                else
                {
                    objectFromDb = _unitOfWork.AlcoholRepository.GetById(objectFromPage.Id);
                    if (objectFromDb == null)
                        return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                    objectFromDb.Name = objectFromPage.Name;
                    objectFromDb.Price = objectFromPage.Price;
                    objectFromDb.IsActive = objectFromPage.IsActive;
                }

                // Creating or Updating object.
                if (IdIsNull)
                    _unitOfWork.AlcoholRepository.Create(objectFromDb);
                else
                    _unitOfWork.AlcoholRepository.Update(objectFromDb);


                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteAlcohol")]
        public async Task<IActionResult> DeleteAlcohol(int Id)
        {
            var objectFromDb = _unitOfWork.AlcoholRepository.GetById(Id);
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
        #region Milk actions.
        [HttpGet("GetMilk")]
        public IActionResult GetMilk(int Id)
        {
            var objectFromDb = _unitOfWork.MilkRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetAllMilk")]
        public async Task<IActionResult> GetAllMilk()
        {
            var objectsFromDb = await _unitOfWork.MilkRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveMilk")]
        public async Task<IActionResult> SaveMilk(Milk objectFromPage)
        {
            if (ModelState.IsValid)
            {
                Milk objectFromDb;
                var IdIsNull = objectFromPage.Id == 0;
                if (IdIsNull)
                    objectFromDb = objectFromPage;
                else
                {
                    objectFromDb = _unitOfWork.MilkRepository.GetById(objectFromPage.Id);
                    if (objectFromDb == null)
                        return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                    objectFromDb.Name = objectFromPage.Name;
                    objectFromDb.Price = objectFromPage.Price;
                    objectFromDb.IsActive = objectFromPage.IsActive;
                }

                // Creating or Updating object.
                if (IdIsNull)
                    _unitOfWork.MilkRepository.Create(objectFromDb);
                else
                    _unitOfWork.MilkRepository.Update(objectFromDb);


                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteMilk")]
        public async Task<IActionResult> DeleteMilk(int Id)
        {
            var objectFromDb = _unitOfWork.MilkRepository.GetById(Id);
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
        #region Sauce actions.
        [HttpGet("GetSauce")]
        public IActionResult GetSauce(int Id)
        {
            var objectFromDb = _unitOfWork.SauceRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetAllSauce")]
        public async Task<IActionResult> GetAllSauce()
        {
            var objectsFromDb = await _unitOfWork.SauceRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveSauce")]
        public async Task<IActionResult> SaveSauce(Sauce objectFromPage)
        {
            if (ModelState.IsValid)
            {
                Sauce objectFromDb;
                var IdIsNull = objectFromPage.Id == 0;
                if (IdIsNull)
                    objectFromDb = objectFromPage;
                else
                {
                    objectFromDb = _unitOfWork.SauceRepository.GetById(objectFromPage.Id);
                    if (objectFromDb == null)
                        return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                    objectFromDb.Name = objectFromPage.Name;
                    objectFromDb.Price = objectFromPage.Price;
                    objectFromDb.IsActive = objectFromPage.IsActive;
                }

                // Creating or Updating object.
                if (IdIsNull)
                    _unitOfWork.SauceRepository.Create(objectFromDb);
                else
                    _unitOfWork.SauceRepository.Update(objectFromDb);


                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSauce")]
        public async Task<IActionResult> DeleteSauce(int Id)
        {
            var objectFromDb = _unitOfWork.SauceRepository.GetById(Id);
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
        #region Supplements actions.
        [HttpGet("GetSupplements")]
        public IActionResult GetSupplements(int Id)
        {
            var objectFromDb = _unitOfWork.SupplementsRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetAllSupplements")]
        public async Task<IActionResult> GetAllSupplements()
        {
            var objectsFromDb = await _unitOfWork.SupplementsRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveSupplements")]
        public async Task<IActionResult> SaveSupplements(Supplements objectFromPage)
        {
            if (ModelState.IsValid)
            {
                Supplements objectFromDb;
                var IdIsNull = objectFromPage.Id == 0;
                if (IdIsNull)
                    objectFromDb = objectFromPage;
                else
                {
                    objectFromDb = _unitOfWork.SupplementsRepository.GetById(objectFromPage.Id);
                    if (objectFromDb == null)
                        return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                    objectFromDb.Name = objectFromPage.Name;
                    objectFromDb.Price = objectFromPage.Price;
                    objectFromDb.IsActive = objectFromPage.IsActive;
                }

                // Creating or Updating object.
                if (IdIsNull)
                    _unitOfWork.SupplementsRepository.Create(objectFromDb);
                else
                    _unitOfWork.SupplementsRepository.Update(objectFromDb);


                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSupplements")]
        public async Task<IActionResult> DeleteSupplements(int Id)
        {
            var objectFromDb = _unitOfWork.SupplementsRepository.GetById(Id);
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
        #region Syrup actions.
        [HttpGet("GetSyrup")]
        public IActionResult GetSyrup(int Id)
        {
            var objectFromDb = _unitOfWork.SyrupRepository.GetById(Id);
            if (objectFromDb == null)
                return NotFound(new JsonResult(new { success = false, message = $"Cannot find object with id = {Id}" }));
            else
                return Ok(new JsonResult(new { data = objectFromDb }));
        }
        [HttpGet("GetAllSyrup")]
        public async Task<IActionResult> GetAllSyrup()
        {
            var objectsFromDb = await _unitOfWork.SyrupRepository.GetAll();
            return Ok(objectsFromDb);
        }
        [HttpPost("SaveSyrup")]
        public async Task<IActionResult> SaveSyrup(Syrup objectFromPage)
        {
            if (ModelState.IsValid)
            {
                Syrup objectFromDb;
                var IdIsNull = objectFromPage.Id == 0;
                if (IdIsNull)
                    objectFromDb = objectFromPage;
                else
                {
                    objectFromDb = _unitOfWork.SyrupRepository.GetById(objectFromPage.Id);
                    if (objectFromDb == null)
                        return NotFound(new JsonResult(new { success = false, message = "Cannot find this object in database" }));
                    objectFromDb.Name = objectFromPage.Name;
                    objectFromDb.Price = objectFromPage.Price;
                    objectFromDb.IsActive = objectFromPage.IsActive;
                }

                // Creating or Updating object.
                if (IdIsNull)
                    _unitOfWork.SyrupRepository.Create(objectFromDb);
                else
                    _unitOfWork.SyrupRepository.Update(objectFromDb);


                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new { success = true, message = "Successfully saved" }));
                else
                    return BadRequest(new JsonResult(new { success = false, message = "Error while saving" }));
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete("DeleteSyrup")]
        public async Task<IActionResult> DeleteSyrup(int Id)
        {
            var objectFromDb = _unitOfWork.SyrupRepository.GetById(Id);
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
