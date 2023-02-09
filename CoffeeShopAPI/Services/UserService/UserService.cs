using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class UserService : ControllerBase, IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IImagesService _imagesService;

        public UserService(IConfiguration config, IUnitOfWork unitOfWork, 
            ILogger<UserService> logger, IImagesService imagesService)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imagesService = imagesService;
        }

        public async Task<IActionResult> Create(User user, IFormFile photo)
        {
            // Creating neccessary variables.
            var type = "User";

            // Building user.
            user.RegistrationDate = DateTime.Now;
            user.ImagePath = $"/Images/{type}/Default{type}Image.png";

            // Creating object.
            _unitOfWork.UserRepository.Create(user);

            // Saving photo.
            if (await _unitOfWork.SaveAsync())
            {
                // Saving photos.
                var imagePath = await _imagesService.SavePhoto(type, photo);

                // Checking result of saving photo.
                if (imagePath == null)
                {
                    _logger.LogInformation("Photo is null");
                    return StatusCode(201, new JsonResult(new
                    {
                        success = true,
                        message = "Successfully saved"
                    }));
                }

                // Setting ImagePath value from amgePath variable. 
                _logger.LogInformation("Photo has been saved");
                user.ImagePath = imagePath;

                // Updating user and saving changes.
                _unitOfWork.UserRepository.Update(user);
                if(await _unitOfWork.SaveAsync())
                    return StatusCode(201, new JsonResult(new
                    {
                        success = true,
                        message = "Successfully saved"
                    }));
            }

            // Returning error while saving result.
            _logger.LogError($"Cannot find object with id = {user.Id}");
            return StatusCode(500, new JsonResult(new
            {
                success = false,
                message = "Error while saving"
            }));
        }

        public async Task<IActionResult> Update(User user, IFormFile photo)
        {
            // Checking existing object.
            var objectFromDb = _unitOfWork.UserRepository.GetById(user.Id);
            if (objectFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {user.Id}");
                return NotFound(new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find user with id {user.Id}"
                }));
            }

            // Checking existing user with same email.
            if (user.Email != objectFromDb.Email)
            {
                var checkEmailUser = await _unitOfWork.UserRepository.GetByEmail(user.Email);
                if (checkEmailUser != null && checkEmailUser.Email == user.Email)
                {
                    _logger.LogError("User with this email is already exists");
                    return Conflict(new JsonResult(new
                    {
                        success = false,
                        message = "User with this email is already exists"
                    }));
                }
            }

            // Updating user.
            objectFromDb.Adress = user.Adress;
            objectFromDb.Email = user.Email;
            objectFromDb.Name = user.Name;
            objectFromDb.Password = user.Password;

            // Updating user.
            _unitOfWork.UserRepository.Update(objectFromDb);
            if (!await _unitOfWork.SaveAsync())
                return BadRequest(new JsonResult(new
                {
                    success = false,
                    message = "Error while updating, photo has not been saved"
                }));


            // Saving photo and and updating user.ImagePath.
            // Saving photo.
            var imagePath = await _imagesService.SavePhoto("User", photo);

            // Chaecking is photo were saved.
            if (imagePath != null)
            {
                _logger.LogInformation("Photo has been saved");
                objectFromDb.ImagePath = imagePath;
                // Saving user with new ImagePath.
                _unitOfWork.UserRepository.Update(objectFromDb);
                var savingresult = await _unitOfWork.SaveAsync();

                if (savingresult)
                    return Ok(new JsonResult(new
                    {
                        success = true,
                        message = "Successfully updated"
                    }));
                else
                    return BadRequest(new JsonResult(new
                    {
                        success = false,
                        message = "Error while updating, Photo has been saved"
                    }));
            }

            return Ok(new JsonResult(new
            {
                success = true,
                message = "Successfully updated"
            }));
        }
    }
}
