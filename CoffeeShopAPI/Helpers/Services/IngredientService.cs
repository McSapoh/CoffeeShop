using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models.Ingredients;
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
        public ServiceResponse GetIngredient(int id, string Type)
        {
            Ingredient ingredient;
            switch (Type)
            {
                case "Alcohol":
                    ingredient = _unitOfWork.AlcoholRepository.GetById(id);
                    break;
                case "Milk":
                    ingredient = _unitOfWork.MilkRepository.GetById(id);
                    break;
                case "Sauce":
                    ingredient = _unitOfWork.SauceRepository.GetById(id);
                    break;
                case "Supplements":
                    ingredient = _unitOfWork.SupplementsRepository.GetById(id);
                    break;
                case "Syrup":
                    ingredient = _unitOfWork.SyrupRepository.GetById(id);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }
            if (ingredient == null)
                return new ServiceResponse(false, $"Cannot find object with id = {id}", 404);
            else
                return new ServiceResponse(true, "", 200) { Data = ingredient };
        }
        public async Task<ServiceResponse> CreateIngredient(Ingredient ingredient, string Type)
        {
            // Creating or Updating object.
            switch (Type)
            {
                case "Alcohol":
                    _unitOfWork.AlcoholRepository.Create((Alcohol)ingredient);
                    break;
                case "Milk":
                    _unitOfWork.MilkRepository.Create((Milk)ingredient);
                    break;
                case "Sauce":
                    _unitOfWork.SauceRepository.Create((Sauce)ingredient);
                    break;
                case "Supplements":
                    _unitOfWork.SupplementsRepository.Create((Supplements)ingredient);
                    break;
                case "Syrup":
                    _unitOfWork.SyrupRepository.Create((Syrup)ingredient);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }

            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully saved", 200);
            else
                return new ServiceResponse(false,"Error while saving", 400);
        }
        public async Task<ServiceResponse> UpdateIngredient(Ingredient ingredient, string Type)
        {
            Ingredient ingredientFromDb;
            // Getting object from database object.
            switch (Type)
            {
                case "Alcohol":
                    ingredientFromDb = _unitOfWork.AlcoholRepository.GetById(ingredient.Id);
                    break;
                case "Milk":
                    ingredientFromDb = _unitOfWork.MilkRepository.GetById(ingredient.Id);
                    break;
                case "Sauce":
                    ingredientFromDb = _unitOfWork.SauceRepository.GetById(ingredient.Id);
                    break;
                case "Supplements":
                    ingredientFromDb = _unitOfWork.SupplementsRepository.GetById(ingredient.Id);
                    break;
                case "Syrup":
                    ingredientFromDb = _unitOfWork.SyrupRepository.GetById(ingredient.Id);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }
            if (ingredientFromDb == null)
                return new ServiceResponse(false, $"Cannot find object with id = {ingredient.Id}", 404);
            ingredientFromDb.Name = ingredient.Name;
            ingredientFromDb.Price = ingredient.Price;
            ingredientFromDb.IsActive = ingredient.IsActive;

            // Updating product.
            switch (Type)
            {
                case "Alcohol":
                    _unitOfWork.AlcoholRepository.Update((Alcohol)ingredientFromDb);
                    break;
                case "Milk":
                    _unitOfWork.MilkRepository.Update((Milk)ingredientFromDb);
                    break;
                case "Sauce":
                    _unitOfWork.SauceRepository.Update((Sauce)ingredientFromDb);
                    break;
                case "Supplements":
                    _unitOfWork.SupplementsRepository.Update((Supplements)ingredientFromDb);
                    break;
                case "Syrup":
                    _unitOfWork.SyrupRepository.Update((Syrup)ingredientFromDb);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }

            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully update", 200);
            else
                return new ServiceResponse(false, "Error while updating", 400);
        }
        public async Task<ServiceResponse> DeleteIngredient(int id, string Type)
        {
            Ingredient ingredient;
            switch (Type)
            {
                case "Alcohol":
                    ingredient = _unitOfWork.AlcoholRepository.GetById(id);
                    break;
                case "Milk":
                    ingredient = _unitOfWork.MilkRepository.GetById(id);
                    break;
                case "Sauce":
                    ingredient = _unitOfWork.SauceRepository.GetById(id);
                    break;
                case "Supplements":
                    ingredient = _unitOfWork.SupplementsRepository.GetById(id);
                    break;
                case "Syrup":
                    ingredient = _unitOfWork.SyrupRepository.GetById(id);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }

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
