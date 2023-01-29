using CoffeeShopAPI.Helpers;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImagesService _imageService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger, IImagesService imagesService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imageService = imagesService;
        }

        public async Task<ServiceResponse> Get(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            
            if (product == null)
                return new ServiceResponse((int)HttpStatusCode.NotFound, new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find object with id = {id}"
                }));
            else
                return new ServiceResponse((int)HttpStatusCode.OK, product);
        }
        public async Task<ServiceResponse> Create(Product product, IFormFile photo)
        {
            // Checking Id for each size
            foreach (var productSize in product.Sizes)
            {
                if (productSize.Id != 0)
                    return new ServiceResponse((int)HttpStatusCode.BadRequest, new JsonResult(new
                    {
                        success = false,
                        message = $"Cannot add size with id = {productSize.Id}"
                    }));
            }

            // Creating object.
            _unitOfWork.ProductRepository.Create(product);

            if (await _unitOfWork.SaveAsync())
            {
                // Saving photos.
                var type = char.ToUpper(product.ProductType.ToString()[0]) + product.ProductType.ToString().Substring(1);
                var imagePath = await _imageService.SavePhoto(type, photo);
                if (imagePath != null)
                    product.ImagePath = imagePath;
                else
                    product.ImagePath = $"/Images/{product.ProductType}/Default{product.ProductType}Image.png";


                return new ServiceResponse((int)HttpStatusCode.Created, new JsonResult(new
                {
                    success = true,
                    message = "Successfully saved"
                }));
            }
            else
                return new ServiceResponse((int)HttpStatusCode.InternalServerError, new JsonResult(new
                {
                    success = false,
                    message = "Error while saving"
                }));
        }
        public async Task<ServiceResponse> Update(Product product, IFormFile photo)
        {
            var productFromDb = await _unitOfWork.ProductRepository.GetByIdAsync(product.Id);

            if (productFromDb == null)
                return new ServiceResponse((int)HttpStatusCode.NotFound, new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find object with id = {product.Id}"
                }));
            productFromDb.Name = product.Name;
            productFromDb.Description = product.Description;
            productFromDb.IsActive = product.IsActive;


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
            {
                // Saving photos.
                var type = char.ToUpper(productFromDb.ProductType.ToString()[0]) + productFromDb.ProductType.ToString().Substring(1);
                var imagePath = await _imageService.SavePhoto(type, photo);
                if (imagePath != null)
                {
                    _imageService.DeletePhoto(productFromDb.ImagePath);
                    productFromDb.ImagePath = imagePath;
                }


                return new ServiceResponse((int)HttpStatusCode.Created, new JsonResult(new
                {
                    success = true,
                    message = "Successfully saved"
                }));
            }
            else
                return new ServiceResponse((int)HttpStatusCode.InternalServerError, new JsonResult(new
                {
                    success = false,
                    message = "Error while saving"
                }));
        }
        public async Task<ServiceResponse> Delete(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product != null)
            {
                if (!product.IsActive)
                    return new ServiceResponse((int)HttpStatusCode.Conflict, new JsonResult(new
                    {
                        success = false,
                        message = "Cannot delete already deleted object"
                    }));
                product.IsActive = false;
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
            return new ServiceResponse((int)HttpStatusCode.NotFound, new JsonResult(new
            {
                success = false,
                message = $"Cannot find object with id = {id}"
            }));
        }
    }
}
