using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class ProductService : ControllerBase, IProductService
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

        public async Task<IActionResult> Get(int id)
        {
            // Getting product from db.
            var productFromDb = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            // Checking productFromDb.
            if (productFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {id}");
                return NotFound();
            }

            return Ok(productFromDb);
        }
        public async Task<IActionResult> Create(Product product, IFormFile photo)
        {
            // Checking Id for each size
            foreach (var productSize in product.Sizes)
            {
                if (productSize.Id != 0)
                {
                    _logger.LogError($"Cannot add size with id = {productSize.Id}");
                    return Conflict(new JsonResult(new
                    {
                        success = false,
                        message = $"Cannot add size with id = {productSize.Id}"
                    }));
                }
            }

            // Building product.
            product.ImagePath = $"{product.ProductType}.png";

            // Creating product.
            _unitOfWork.ProductRepository.Create(product);

            if (await _unitOfWork.SaveAsync())
            {
                // Returning Created if photo was not saved.
                if (photo == null)
                    return StatusCode(201);

                // Saving photos.
                var type = char.ToUpper(product.ProductType.ToString()[0]) + product.ProductType.ToString().Substring(1);
                var imagePath = await _imageService.SavePhoto(photo);
                product.ImagePath = imagePath;

                // Updating product.ImagePath.
                _unitOfWork.ProductRepository.Update(product);

                if (await _unitOfWork.SaveAsync())
                    return StatusCode(201); 
            }

            _logger.LogError("Unknown error occurred while creating");
            return StatusCode(500);
        }
        public async Task<IActionResult> Update(Product product, IFormFile photo)
        {
            var productFromDb = await _unitOfWork.ProductRepository.GetByIdAsync(product.Id);

            if (productFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {product.Id}");
                return NotFound();
            }
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
                // Returning Created if photo was not saved.
                if (photo == null)
                    return StatusCode(201);

                // Saving photos.
                var imagePath = await _imageService.SavePhoto(photo);
                if (imagePath != null)
                {
                    _imageService.DeletePhoto(productFromDb.ImagePath);
                    productFromDb.ImagePath = imagePath;
                }

                // Updating product.ImagePath.
                _unitOfWork.ProductRepository.Update(productFromDb);

                if (await _unitOfWork.SaveAsync())
                    return StatusCode(201);
            }

            _logger.LogError("Unknown error occurred while creating");
            return StatusCode(500);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var productFromDb = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            
            // Checking productFromDb.
            if (productFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {id}");
                return NotFound();
            }

            if (!productFromDb.IsActive)
            {
                _logger.LogError("Cannot delete already deleted object");
                return Conflict(new JsonResult(new
                {
                    success = false,
                    message = "Cannot delete already deleted object"
                }));
            }

            productFromDb.IsActive = false;

            if (await _unitOfWork.SaveAsync())
                return Ok();

            _logger.LogError("Unknown error occurred while creating");
            return StatusCode(500);
        }
    }
}
