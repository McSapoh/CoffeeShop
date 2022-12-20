﻿using CoffeeShopAPI.Interfaces.Repositories;
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

            // Updating product.
            if (product.Sizes.Count > 0)
            {
                if (productFromDb.Sizes.Count == 5)
                {
                    // Updating sizes.
                    foreach (var productFromDbSize in productFromDb.Sizes)
                    {
                        foreach (var productSize in product.Sizes)
                        {
                            productSize.ProductId = productFromDb.Id;
                            if (productFromDbSize.Id == productSize.Id)
                            {
                                productFromDbSize.Name = productSize.Name;
                                productFromDbSize.Description = productSize.Description;
                                productFromDbSize.Price = productSize.Price;
                                _unitOfWork.SizeRepository.Update(productFromDbSize);
                            }
                        }
                    }
                }
                else if (productFromDb.Sizes.Count < 5)
                {
                    // Updating sizes.
                    foreach (var productFromDbSize in productFromDb.Sizes)
                    {
                        foreach (var productSize in product.Sizes)
                        {
                            productSize.ProductId = productFromDb.Id;
                            if (productFromDbSize.Id == productSize.Id)
                                _unitOfWork.SizeRepository.Update(productSize);
                        }
                    }
                    // Creating sizes.
                    foreach (var productSize in product.Sizes)
                    {
                        if (productSize.Id != 0 && productFromDb.Sizes.Count < 5)
                        {
                            productSize.ProductId = productFromDb.Id;
                            _unitOfWork.SizeRepository.Update(productSize);
                            productFromDb.Sizes.Add(productSize);
                        }
                    }
                }

                //foreach (var productSize in product.Sizes)
                //{


                //    foreach (var productFromDbSize in productFromDb.Sizes)
                //    {
                //        if (productFromDbSize.Id == productSize.Id)
                //        {
                //            _unitOfWork.SizeRepository.Update(productSize);
                //        }
                //    }
                //    if (productSize.Id == 0)
                //        _unitOfWork.SizeRepository.Create(productSize);
                //    else
                //        _unitOfWork.SizeRepository.Update(productSize);
                //}
            }
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