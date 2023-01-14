using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Helpers.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImagesService _imageService;


        public ProductService(IUnitOfWork unitOfWork, IImagesService imagesService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imagesService;
        }

        public async Task<ServiceResponse> Get(int id)
        {
            if (id <= 0)
                return new ServiceResponse(false, "Invalid id", 400);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            
            if (product == null)
                return new ServiceResponse(false, $"Cannot find object with id = {id}", 404);
            else
                return new ServiceResponse(true, "", 200) { Data = product };
        }
        public async Task<ServiceResponse> Create(Product product, IFormFile photo)
        {
            // Saving photos.
            var type = char.ToUpper(product.ProductType.ToString()[0]) + product.ProductType.ToString().Substring(1);
            var imagePath = await _imageService.SavePhoto(type, photo);
            if (imagePath != null)
                product.ImagePath = imagePath;
            else
                product.ImagePath = $"/Images/{product.ProductType}/Default{product.ProductType}Image.png";

            // Checking Id for each size
            foreach (var productSize in product.Sizes)
            {
                if (productSize.Id != 0)
                    return new ServiceResponse(false, $"Cannot add size with id = {productSize.Id}", 400);
            }

            // Creating object.
            _unitOfWork.ProductRepository.Create(product);

            if (await _unitOfWork.SaveAsync())
                return new ServiceResponse(true, "Successfully saved", 200);
            else
                return new ServiceResponse(false,"Error while saving", 400);
        }
        public async Task<ServiceResponse> Update(Product product, IFormFile photo)
        {
            var productFromDb = await _unitOfWork.ProductRepository.GetByIdAsync(product.Id);

            if (productFromDb == null)
                return new ServiceResponse(false, $"Cannot find object with id = {product.Id}", 404);
            productFromDb.Name = product.Name;
            productFromDb.Description = product.Description;
            productFromDb.IsActive = product.IsActive;


            // Saving photos.
            var type = char.ToUpper(productFromDb.ProductType.ToString()[0]) + productFromDb.ProductType.ToString().Substring(1);
            var imagePath = await _imageService.SavePhoto(type, photo);
            if (imagePath != null)
            {
                _imageService.DeletePhoto(productFromDb.ImagePath);
                productFromDb.ImagePath = imagePath;
            }

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
            if (id <= 0)
                return new ServiceResponse(false, "Invalid id", 400);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

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
