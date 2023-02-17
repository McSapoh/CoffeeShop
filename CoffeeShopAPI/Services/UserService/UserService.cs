using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.User;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class UserService : ControllerBase, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IImagesService _imagesService;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, 
            IImagesService imagesService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imagesService = imagesService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Get(int id)
        {
            // Getting userFromDb.
            var userFromDb = await _unitOfWork.UserRepository.GetByIdAsync(id);

            // Checking userFromDb.
            if (userFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {id}");
                return NotFound();
            }

            var UserDTO = _mapper.Map<DisplayUserDTO>(userFromDb);
            return Ok(UserDTO);
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
                    return StatusCode(201);
                }

                // Setting ImagePath value from amgePath variable. 
                _logger.LogInformation("Photo has been saved");
                user.ImagePath = imagePath;

                // Updating user and saving changes.
                _unitOfWork.UserRepository.Update(user);
                if(await _unitOfWork.SaveAsync())
                    return StatusCode(201);
            }

            // Returning error while saving result.
            _logger.LogError($"Cannot find object with id = {user.Id}");
            return StatusCode(500);
        }

        public async Task<IActionResult> Update(User user, IFormFile photo)
        {
            // Checking existing object.
            var objectFromDb = _unitOfWork.UserRepository.GetById(user.Id);
            if (objectFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {user.Id}");
                return NotFound();
            }

            // Checking existing user with same email.
            if (user.Email != objectFromDb.Email)
            {
                var checkEmailUser = await _unitOfWork.UserRepository.GetByEmail(user.Email);
                if (checkEmailUser != null && checkEmailUser.Email == user.Email)
                {
                    _logger.LogError("User with this email is already exists");
                    return Conflict();
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
                return StatusCode(500);


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
                    return StatusCode(201);
                else
                    return StatusCode(500);
            }

            return StatusCode(201);
        }
    }
}
