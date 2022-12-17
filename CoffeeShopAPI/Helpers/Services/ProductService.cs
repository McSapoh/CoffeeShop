using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models.Products;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Helpers.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
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

        public ServiceResponse GetProduct(int Id, string Type)
        {
            Product product;
            switch (Type)
            {
                case "Coffee":
                    product = _unitOfWork.CoffeeRepository.GetById(Id);
                    break;
                case "Dessert":
                    product = _unitOfWork.DessertRepository.GetById(Id);
                    break;
                case "Sandwich":
                    product = _unitOfWork.SandwichRepository.GetById(Id);
                    break;
                case "Snack":
                    product = _unitOfWork.SnackRepository.GetById(Id);
                    break;
                case "Tea":
                    product = _unitOfWork.TeaRepository.GetById(Id);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }
            if (product == null)
                return new ServiceResponse(false, $"Cannot find object with id = {Id}", 404);
            else
                return new ServiceResponse(true, "", 200) { Data = product };
        }
        public async Task<ServiceResponse> CreateProduct(Product product, IFormFile photo, string Type)
        {
            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + $"\\Images\\{Type}", photo);
                product.ImagePath = $"/Images/{Type}/" + photo.FileName;
            }
            else
                product.ImagePath = $"/Images/{Type}/Default{Type}Image.png";

            // Creating or Updating object.
            switch (Type)
            {
                case "Coffee":
                    _unitOfWork.CoffeeRepository.Create((Coffee)product);
                    break;
                case "Dessert":
                    _unitOfWork.DessertRepository.Create((Dessert)product);
                    break;
                case "Sandwich":
                    _unitOfWork.SandwichRepository.Create((Sandwich)product);
                    break;
                case "Snack":
                    _unitOfWork.SnackRepository.Create((Snack)product);
                    break;
                case "Tea":
                    _unitOfWork.TeaRepository.Create((Tea)product);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }

            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully saved", 200);
            else
                return new ServiceResponse(false,"Error while saving", 400);
        }
        public async Task<ServiceResponse> UpdateProduct(Product product, IFormFile photo, string Type)
        {
            Product productFromDb;
            // Getting object from database object.
            switch (Type)
            {
                case "Coffee":
                    productFromDb = _unitOfWork.CoffeeRepository.GetById(product.Id);
                    break;
                case "Dessert":
                    productFromDb = _unitOfWork.DessertRepository.GetById(product.Id);
                    break;
                case "Sandwich":
                    productFromDb = _unitOfWork.SandwichRepository.GetById(product.Id);
                    break;
                case "Snack":
                    productFromDb = _unitOfWork.SnackRepository.GetById(product.Id);
                    break;
                case "Tea":
                    productFromDb = _unitOfWork.TeaRepository.GetById(product.Id);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }
            if (productFromDb == null)
                return new ServiceResponse(false, $"Cannot find object with id = {product.Id}", 404);
            productFromDb.Name = product.Name;
            productFromDb.Description = product.Description;
            productFromDb.IsActive = product.IsActive;

            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + $"\\Images\\{Type}", photo);
                if (productFromDb.ImagePath != $"/{Type}/Default{Type}Image.png")
                    DeletePhoto(path + "\\Images" + productFromDb.ImagePath);
                productFromDb.ImagePath = $"/Images/{Type}/" + photo.FileName;
            }
            else
                productFromDb.ImagePath = $"/{Type}/Default{Type}Image.png";

            // Updating product.
            switch (Type)
            {
                case "Coffee":
                    _unitOfWork.CoffeeRepository.Update((Coffee)productFromDb);
                    break;
                case "Dessert":
                    _unitOfWork.DessertRepository.Update((Dessert)productFromDb);
                    break;
                case "Sandwich":
                    _unitOfWork.SandwichRepository.Update((Sandwich)productFromDb);
                    break;
                case "Snack":
                    _unitOfWork.SnackRepository.Update((Snack)productFromDb);
                    break;
                case "Tea":
                    _unitOfWork.TeaRepository.Update((Tea)productFromDb);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }

            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully update", 200);
            else
                return new ServiceResponse(false, "Error while updating", 400);
        }
        public async Task<ServiceResponse> DeleteProduct(int id, string Type)
        {
            Product product;
            switch (Type)
            {
                case "Coffee":
                    product = _unitOfWork.CoffeeRepository.GetById(id);
                    break;
                case "Dessert":
                    product = _unitOfWork.DessertRepository.GetById(id);
                    break;
                case "Sandwich":
                    product = _unitOfWork.SandwichRepository.GetById(id);
                    break;
                case "Snack":
                    product = _unitOfWork.SnackRepository.GetById(id);
                    break;
                case "Tea":
                    product = _unitOfWork.TeaRepository.GetById(id);
                    break;
                default:
                    return new ServiceResponse(false, $"Cannot find object type {Type}", 404);
            }

            if (product != null)
            {
                if (!product.IsActive)
                    return new ServiceResponse(false, "Cannot delete already deleted object", 400);
                product.IsActive = false;
                if (await _unitOfWork.SaveAsync())
                    return new ServiceResponse(true, "Successfully deleted", 200);
                else
                    return new ServiceResponse(false, "Error while deleting", 400);
            }
            return new ServiceResponse(false, $"Cannot find object with id = {id}", 404);
        }
    }
}
