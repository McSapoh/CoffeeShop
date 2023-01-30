using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IngredientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> Get(int id)
        {
            // Getting item.
            var ingredient = await _unitOfWork.IngredientRepository.GetByIdAsync(id);

            // Validation.
            if (ingredient == null)
                return new ServiceResponse((int)HttpStatusCode.NotFound, new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find object with id = {id}"
                }));
            else
                return new ServiceResponse((int)HttpStatusCode.OK, ingredient);
        }
        public async Task<ServiceResponse> Create(Ingredient ingredient)
        {
            // Creating object.
            _unitOfWork.IngredientRepository.Create(ingredient);

            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse((int)HttpStatusCode.Created, new JsonResult(new
                {
                    success = true,
                    message = "Successfully saved"
                }));
            else
                return new ServiceResponse((int)HttpStatusCode.InternalServerError, new JsonResult(new
                {
                    success = false,
                    message = "Error while saving"
                }));
        }
        public async Task<ServiceResponse> Update(Ingredient ingredient)
        {
            // Getting item.
            var ingredientFromDb  = await _unitOfWork.IngredientRepository.GetByIdAsync(ingredient.Id);

            // Validation.
            if (ingredientFromDb == null)
                return new ServiceResponse((int)HttpStatusCode.NotFound, new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find object with id = {ingredient.Id}"
                }));

            // Changing.
            ingredientFromDb.Name = ingredient.Name;
            ingredientFromDb.Price = ingredient.Price;
            ingredientFromDb.IsActive = ingredient.IsActive;

            // Updating item.
            _unitOfWork.IngredientRepository.Update(ingredientFromDb);
                    
            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse((int)HttpStatusCode.Created, new JsonResult(new
                {
                    success = true,
                    message = "Successfully saved"
                }));
            else
                return new ServiceResponse((int)HttpStatusCode.InternalServerError, new JsonResult(new
                {
                    success = false,
                    message = "Error while saving"
                }));
        }
        public async Task<ServiceResponse> Delete(int id)
        {
            // Getting item.
            var ingredient = _unitOfWork.IngredientRepository.GetById(id);

            // Validation.
            if (ingredient != null)
                return new ServiceResponse((int)HttpStatusCode.NotFound, new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find object with id = {id}"
                }));
            
            if (!ingredient.IsActive)
                return new ServiceResponse((int)HttpStatusCode.Conflict, new JsonResult(new
                {
                    success = false,
                    message = "Cannot delete already deleted object"
                }));


            // Action.
            ingredient.IsActive = false;
            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse((int)HttpStatusCode.OK, new JsonResult(new
                {
                    success = true,
                    message = "Successfully deleted"
                }));
            else
                return new ServiceResponse((int)HttpStatusCode.InternalServerError, new JsonResult(new
                {
                    success = false,
                    message = "Error while deleting"
                }));
        }
    }
}
