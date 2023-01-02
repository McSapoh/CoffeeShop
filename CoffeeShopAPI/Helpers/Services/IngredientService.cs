using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Helpers.Services
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
            if (id <= 0)
                return new ServiceResponse(false, "Invalid id", 400);

            var ingredient = await _unitOfWork.IngredientRepository.GetByIdAsync(id);

            if (ingredient == null)
                return new ServiceResponse(false, $"Cannot find object with id = {id}", 404);
            else
                return new ServiceResponse(true, "", 200) { Data = ingredient };
        }
        public async Task<ServiceResponse> Create(Ingredient ingredient)
        {
            // Creating object.
            _unitOfWork.IngredientRepository.Create(ingredient);

            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully saved", 200);
            else
                return new ServiceResponse(false,"Error while saving", 400);
        }
        public async Task<ServiceResponse> Update(Ingredient ingredient)
        {
            var ingredientFromDb  = await _unitOfWork.IngredientRepository.GetByIdAsync(ingredient.Id);
                   
            if (ingredientFromDb == null)
                return new ServiceResponse(false, $"Cannot find object with id = {ingredient.Id}", 404);
            ingredientFromDb.Name = ingredient.Name;
            ingredientFromDb.Price = ingredient.Price;
            ingredientFromDb.IsActive = ingredient.IsActive;

            // Updating product.
            _unitOfWork.IngredientRepository.Update(ingredientFromDb);
                    
            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully update", 200);
            else
                return new ServiceResponse(false, "Error while updating", 400);
        }
        public async Task<ServiceResponse> Delete(int id)
        {
            if (id <= 0)
                return new ServiceResponse(false, "Invalid id", 400);

            var ingredient = _unitOfWork.IngredientRepository.GetById(id);
                    
            if (ingredient != null)
            {
                if (!ingredient.IsActive)
                    return new ServiceResponse(false, "Cannot delete already deleted object", 400);
                ingredient.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return new ServiceResponse(true, "Successfully deleted", 200);
                else
                    return new ServiceResponse(false, "Error while deleting", 400);
            }
            return new ServiceResponse(false, $"Cannot find object with id = {id}", 404);
        }
    }
}
