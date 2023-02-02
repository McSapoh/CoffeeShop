using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.User;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImagesService _imagesService;
        protected readonly ILogger<UsersController> _logger;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IImagesService imagesService,
            ILogger<UsersController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagesService = imagesService;
            _logger = logger;
        }


        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] EditUserDTO objectFromPage, IFormFile photo)
        {
            _logger.LogInformation($"PUT {this}.Update called.");

            // Validation.
            if (photo != null && !photo.ContentType.Contains("image"))
            {
                _logger.LogError("Parameter photo is not an image");
                return BadRequest(new JsonResult(new { success = false, message = "File is not a photo" }));
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is not valid");
                return BadRequest(ModelState);
            }

            // Checking existing user with same email.
            if (_unitOfWork.UserRepository.GetByEmail(objectFromPage.Email) != null)
            {
                _logger.LogError("User with this email is already exists");
                return BadRequest(new JsonResult(new
                {
                    success = false,
                    message = "User with this email is already exists"
                }));
            }

            // Building user.
            var user = _mapper.Map<User>(objectFromPage);
            var imagePath = await _imagesService.SavePhoto("User", objectFromPage.Photo);
            if (imagePath != null)
                user.ImagePath = imagePath;
            user.RegistrationDate = DateTime.Now;

            // Creating user.
            _unitOfWork.UserRepository.Create(user);
            var savingresult = await _unitOfWork.SaveAsync();
            _logger.LogInformation($"PUT {this}.Update finished with result {savingresult}.");
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
                    message = "Error while updating"
                }));
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] EditUserDTO objectFromPage, IFormFile photo)
        {
            _logger.LogInformation($"PUT {this}.Update called.");

            // Validation.
            if (photo != null && !photo.ContentType.Contains("image"))
            {
                _logger.LogError("Parameter photo is not an image");
                return BadRequest(new JsonResult(new { success = false, message = "File is not a photo" }));
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is not valid");
                return BadRequest(ModelState);
            }

            // Checking existing object.
            var objectFromDb = _unitOfWork.UserRepository.GetById(id);
            if (objectFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {id}");
                return NotFound(new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find user with id {id}"
                }));
            }

            // Checking existing user with same email.
            if (objectFromPage.Email != objectFromDb.Email)
            {
                var checkEmailUser = _unitOfWork.UserRepository.GetByEmail(objectFromPage.Email);
                if (checkEmailUser != null && checkEmailUser.Email == objectFromPage.Email)
                {
                    _logger.LogError("User with this email is already exists");
                    return BadRequest(new JsonResult(new
                    {
                        success = false,
                        message = "User with this email is already exists"
                    }));
                }
            }

            // Updating user.
            var user = _mapper.Map<User>(objectFromPage);
            objectFromDb.Adress = user.Adress;
            objectFromDb.Email = user.Email;
            objectFromDb.Name = user.Name;
            objectFromDb.Password = user.Password;

            _unitOfWork.UserRepository.Update(objectFromDb);
            if (await _unitOfWork.SaveAsync())
            {
                // Saving photo.
                var imagePath = await _imagesService.SavePhoto("User", objectFromPage.Photo);

                // Chaecking is photo were saved.
                if (imagePath != null)
                {
                    objectFromDb.ImagePath = imagePath;
                    // Saving user with new ImagePath.
                    _unitOfWork.UserRepository.Update(objectFromDb);
                    var savingresult = await _unitOfWork.SaveAsync();
                    _logger.LogInformation($"PUT {this}.Update finished with result {savingresult}.");
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
                            message = "Error while updating"
                        }));
                }
                _logger.LogInformation($"PUT {this}.Update finished.");
                return Ok(new JsonResult(new
                {
                    success = true,
                    message = "Successfully updated"
                }));
            }
            else
            {
                _logger.LogInformation($"PUT {this}.Update finished.");
                return BadRequest(new JsonResult(new
                {
                    success = false,
                    message = "Error while updating"
                }));
            }
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.UserRepository
                .GetPagedList(pagingParameters);
            var listObjectsFromDb = _mapper.Map<List<DisplayUserDTO>>(objectsFromDb);
            var convertedObjects = new PagedList<DisplayUserDTO>(listObjectsFromDb, objectsFromDb.Count,
                objectsFromDb.CurrentPage, objectsFromDb.PageSize);
            var metadata = PagedList<DisplayUserDTO>.GetMetadata(convertedObjects);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(convertedObjects);
        }
    }
}
