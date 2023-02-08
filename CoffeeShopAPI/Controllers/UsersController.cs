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

        /// <summary>
        /// Logs in and returns JWT as response and refresh token in cookies.
        /// </summary>
        /// <response code="200">Returns JWT and refresh token</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="404">If cannot find user with current email</response>
        /// <response code="409">If password is incorrect</response>
        /// <response code="500">If unknown error occurred while editing refresh token</response>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserDTO userFromPage)
        {
            _logger.LogInformation($"POST {this}.Login called.");

            // Validation.
            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is not valid");
                return BadRequest(ModelState);
            }

            // Getting user from db.
            var userFromDb = await _unitOfWork.UserRepository.GetByEmail(userFromPage.Email);

            // Validation user from db.
            if (userFromDb == null)
            {
                _logger.LogError($"POST {this}.Login cannot find user with current email");
                return NotFound();
            }
            if (userFromDb.Password != userFromPage.Password)
            {
                _logger.LogError($"POST {this}.Login password doesnt match");
                return Conflict();
            }

            // Getting tokens.
            var JWTToken = _userService.GenerateJWT(userFromDb);
            var RefreshToken = _userService.GenerateRefreshToken();

            // Appending RefreshToken to response.
            // Setting cookies.
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = RefreshToken.Expires
            };

            // Appending data to response.
            Response.Cookies.Append("refreshToken", RefreshToken.Token, cookieOptions);


            // Appending RefreshToken to userFromDB.
            userFromDb.RefreshToken.Created = RefreshToken.Created;
            userFromDb.RefreshToken.Expires = RefreshToken.Expires;
            userFromDb.RefreshToken.Token = RefreshToken.Token;

            // Updating userFromDb and saving changes into db.
            _unitOfWork.UserRepository.Update(userFromDb);
            var savingresult = await _unitOfWork.SaveAsync();

            if (savingresult)
                return Ok(JWTToken);
            else
                return StatusCode(500, new JsonResult(new
                {
                    success = false,
                    message = "Error while updating, Photo has been saved"
                }));
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

            // Updating user.
            var result = await _userService.Update(user, photo);

            _logger.LogInformation($"PUT {this}.Update finished.");

            return result;
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
            user.Id = id;
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
