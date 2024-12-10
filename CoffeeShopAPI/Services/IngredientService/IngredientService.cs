using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class IngredientService : ControllerBase, IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger<IngredientService> _logger;

        public IngredientService(IUnitOfWork unitOfWork, ILogger<IngredientService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IActionResult> Get(int id)
        {
            // Getting item.
            var ingredient = await _unitOfWork.IngredientRepository.GetByIdAsync(id);

            // Validation.
            if (ingredient == null)
            {
                _logger.LogError($"Cannot find object with id = {id}");
                return NotFound();
            }

            return Ok(ingredient);
        }
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            // Creating object.
            _unitOfWork.IngredientRepository.Create(ingredient);

            if (await _unitOfWork.SaveAsync())
                return StatusCode(201);

            _logger.LogError("Unknown error occurred while creating");
            return StatusCode(500);
        }
        public async Task<IActionResult> Update(Ingredient ingredient)
        {
            // Getting item.
            var ingredientFromDb  = await _unitOfWork.IngredientRepository.GetByIdAsync(ingredient.Id);

            // Validation.
            if (ingredientFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {ingredientFromDb.Id}");
                NotFound();
            }

            if (ingredientFromDb.IngredientType != ingredient.IngredientType)
            {
                _logger.LogError($"Cannot change object IngredientType");
                return Conflict(new JsonResult(new
                {
                    success = false,
                    message = $"Cannot change object IngredientType"
                }));
            }

            // Changing.
            ingredientFromDb.Name = ingredient.Name;
            ingredientFromDb.Price = ingredient.Price;
            ingredientFromDb.IsActive = ingredient.IsActive;

            // Updating item.
            _unitOfWork.IngredientRepository.Update(ingredientFromDb);

            if (await _unitOfWork.SaveAsync())
                return StatusCode(201);

            _logger.LogError("Unknown error occurred while creating");
            return StatusCode(500);
        }
        public async Task<IActionResult> Delete(int id)
        {
            // Getting item.
            var ingredient = _unitOfWork.IngredientRepository.GetById(id);

            // Validation.
            if (ingredient == null)
            {
                _logger.LogError($"Cannot find object with id = {id}");
                return NotFound();
            }

            if (!ingredient.IsActive)
            {
                _logger.LogError("Cannot delete already deleted object");
                return Conflict(new JsonResult(new
                {
                    success = false,
                    message = "Cannot delete already deleted object"
                }));
            }


            // Action.
            ingredient.IsActive = false;

            if (await _unitOfWork.SaveAsync())
                return Ok();

            _logger.LogError("Unknown error occurred while creating");
            return StatusCode(500);
        }
    }
}
