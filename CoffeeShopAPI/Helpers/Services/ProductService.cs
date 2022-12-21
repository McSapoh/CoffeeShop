using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
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

        public ServiceResponse Get(int id, string type)
        {
            Product product;
            product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
                return new ServiceResponse(false, $"Cannot find object with id = {id}", 404);
            else
                return new ServiceResponse(true, "", 200) { Data = product };
        }
        public async Task<ServiceResponse> Create(Product product, IFormFile photo)
        {
            // Saving photos.
            if (photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + $"\\Images\\{product.ProductType}", photo);
                product.ImagePath = $"/Images/{product.ProductType}/" + photo.FileName;
            }
            else
                product.ImagePath = $"/Images/{product.ProductType}/Default{product.ProductType}Image.png";

            // Creating or Updating object.
            _unitOfWork.ProductRepository.Create(product);
            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully saved", 200);
            else
                return new ServiceResponse(false,"Error while saving", 400);
        }
        public async Task<ServiceResponse> Update(Product product, IFormFile photo)
        {
            var productFromDb = _unitOfWork.ProductRepository.GetById(product.Id);

            if (productFromDb == null)
                return new ServiceResponse(false, $"Cannot find object with id = {product.Id}", 404);
            productFromDb.Name = product.Name;
            productFromDb.Description = product.Description;
            productFromDb.IsActive = product.IsActive;
                        

            // Saving photos.
            if (productFromDb.ImagePath != null && photo != null && photo.Length > 0)
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                SavePhoto(path + $"\\Images\\{product.ProductType}", photo);
                if (productFromDb.ImagePath != $"/{product.ProductType}/Default{product.ProductType}Image.png")
                    DeletePhoto(path + "\\Images" + productFromDb.ImagePath);
                productFromDb.ImagePath = $"/Images/{product.ProductType}/" + photo.FileName;
            }
            else
                productFromDb.ImagePath = $"/{product.ProductType}/Default{product.ProductType}Image.png";

            // Disactivating existing sizes.
            foreach (var productFromDbSize in productFromDb.Sizes)
            {
                productFromDbSize.IsActive = false;
                _unitOfWork.SizeRepository.Update(productFromDbSize);
            }
            // Updating / Creating sizes
            foreach (var productSize in product.Sizes)
            {
                var updated = false;
                productSize.ProductId = productFromDb.Id;
                foreach (var productFromDbSize in productFromDb.Sizes)
                {
                    // Updating size
                    if (productFromDbSize.Id != 0 && productFromDbSize.Id == productSize.Id)
                    {
                        productFromDbSize.Name = productSize.Name;
                        productFromDbSize.Description = productSize.Description;
                        productFromDbSize.Price = productSize.Price;
                        productFromDbSize.IsActive = true;
                        _unitOfWork.SizeRepository.Update(productFromDbSize);
                        updated = true;
                        break;
                    }
                }
                // Creating size
                if (!updated)
                {
                    productFromDb.Sizes.Add(productSize);
                    _unitOfWork.SizeRepository.Create(productSize);
                }
            }

            // Updating product.
            _unitOfWork.ProductRepository.Update(productFromDb);
            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully update", 200);
            else
                return new ServiceResponse(false, "Error while updating", 400);
        }
        public async Task<ServiceResponse> Delete(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);


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
