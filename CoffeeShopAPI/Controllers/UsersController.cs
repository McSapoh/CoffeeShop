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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]"), Authorize]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly ILogger<UsersController> _logger;
        protected readonly IUserService _userService;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<UsersController> logger, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
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
        public async Task<IActionResult> Update(int id, [FromForm] EditUserDTO objectFromPage, IFormFile photo)
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
        /// Gets user.
        /// </summary>
        /// <response code="200">Returns user from db</response>
        /// <response code="400">If the id parameter is not int</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the user from db is null</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"GET({id}) {this}.Get called.");
            var result = await _userService.Get(id);
            _logger.LogInformation($"GET({id}) {this}.Get finished.");
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
        public IActionResult Get([FromQuery] PagingParameters pagingParameters)
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
