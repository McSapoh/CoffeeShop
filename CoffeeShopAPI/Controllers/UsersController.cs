using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.User;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]"), Authorize]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImagesService _imagesService;
        protected readonly ILogger<UsersController> _logger;
        protected readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IImagesService imagesService,
            ILogger<UsersController> logger, IUserService userService, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagesService = imagesService;
            _logger = logger;
            _userService = userService;
            _authService = authService;
        }

        /// <summary>
        /// Logs in and returns JWT as response and refresh token in cookies.
        /// </summary>
        /// <response code="200">Returns JWT and refresh token</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If cannot find user with current email</response>
        /// <response code="409">If password is incorrect</response>
        /// <response code="500">If unknown error occurred while editing refresh token</response>
        [HttpPost("Login"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
            var JWTToken = _authService.GenerateJWT(userFromDb);
            var RefreshToken = _authService.GenerateRefreshToken();

            // Appending RefreshToken to response.
            // Setting cookies.
            _authService.AppendRefreshTokenToResponse(RefreshToken, Response);


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

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <response code="201">If the user successfully created</response>        
        /// <response code="400">If the photo is not an image or model is not valid</response>
        /// <response code="401">Unathorized</response>
        /// <response code="500">If unknown error occurred while creating</response>
        [HttpPost("Register"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromForm] EditUserDTO objectFromPage, IFormFile photo)
        {
            _logger.LogInformation($"POST {this}.Update called.");

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

            // Building user.
            var user = _mapper.Map<User>(objectFromPage);

            // Updating user.
            var result = await _userService.Update(user, photo);

            _logger.LogInformation($"POST {this}.Update finished.");

            return result;
        }

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <response code="201">If the user successfully updated</response>        
        /// <response code="400">If the photo is not an image or model is not valid</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the user from db is null</response>
        /// <response code="409">If user with the same email is already exists</response>
        /// <response code="500">If unknown error occurred while updating</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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

        /// <summary>
        /// Gets paged list of products.
        /// </summary>
        /// <response code="401">Unathorized</response>
        /// <response code="200">Returns paged list of porudcts</response>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
