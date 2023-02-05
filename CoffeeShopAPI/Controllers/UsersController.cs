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
        protected readonly IUserService _userService;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IImagesService imagesService,
            ILogger<UsersController> logger, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagesService = imagesService;
            _logger = logger;
            _userService = userService;
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
            if (await _unitOfWork.UserRepository.GetByEmail(objectFromPage.Email) != null)
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

            var user = _mapper.Map<User>(objectFromPage);
            var result = await _userService.Update(user, photo);

            _logger.LogInformation($"PUT {this}.Update finished.");

            return result;
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
