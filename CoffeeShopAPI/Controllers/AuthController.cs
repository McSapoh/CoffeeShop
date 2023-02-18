﻿using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.User;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]"), Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly ILogger<AuthController> _logger;
        protected readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService,
            ILogger<AuthController> logger, IUserService userService, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
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
        /// Logs in and returns JWT as response and refresh token in cookies.
        /// </summary>
        /// <response code="200">Returns JWT and refresh token</response>
        /// <response code="401">Unathorized</response>
        /// <response code="500">If unknown error occurred while editing refresh token</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> RefreshToken()
        {
            _logger.LogInformation($"POST {this}.RefreshToken called.");

            // Getting user from context
            var user = await _authService.GetUserByIdentity(HttpContext);

            // Getting refreshToken from cookies.
            var OldRefreshToken = Request.Cookies["refreshToken"];

            // Validation.
            if (!user.RefreshToken.Token.Equals(OldRefreshToken))
            {
                _logger.LogError("Invalid Refresh Token");
                return Unauthorized("Invalid Refresh Token");
            }
            else if (user.RefreshToken.Expires < DateTime.Now)
            {
                _logger.LogError("Token expired");
                return Unauthorized("Token expired");
            }
            // Getting tokens.
            var JWTToken = _authService.GenerateJWT(user);
            var NewRefreshToken = _authService.GenerateRefreshToken();

            // Appending RefreshToken to response.
            _authService.AppendRefreshTokenToResponse(NewRefreshToken, Response);


            // Appending RefreshToken to userFromDB.
            user.RefreshToken.Created = NewRefreshToken.Created;
            user.RefreshToken.Expires = NewRefreshToken.Expires;
            user.RefreshToken.Token = NewRefreshToken.Token;

            // Updating userFromDb and saving changes into db.
            _unitOfWork.UserRepository.Update(user);
            var savingresult = await _unitOfWork.SaveAsync();

            _logger.LogInformation($"POST {this}.RefreshToken finished with result {savingresult}.");

            if (savingresult)
                return Ok(JWTToken);
            else
                return StatusCode(500, new JsonResult(new
                {
                    success = false,
                    message = "Error while updating token in db"
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

    }
}