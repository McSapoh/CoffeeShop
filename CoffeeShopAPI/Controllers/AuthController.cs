using AutoMapper;
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
            HttpContext.Session.SetString("email", userFromDb.Email);

            // Appending RefreshToken to userFromDB.
            if (userFromDb.RefreshToken != null)
            {
                userFromDb.RefreshToken.Created = RefreshToken.Created;
                userFromDb.RefreshToken.Expires = RefreshToken.Expires;
                userFromDb.RefreshToken.Token = RefreshToken.Token;
            }
            else
            {
                userFromDb.RefreshToken = new RefreshToken
                {
                    Created = RefreshToken.Created,
                    Expires = RefreshToken.Expires,
                    Token = RefreshToken.Token
                };
            }

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
        [HttpPost("refresh-token"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> RefreshToken()
        {
            _logger.LogInformation($"POST {this}.RefreshToken called.");

            // Getting user from context
            string email = HttpContext.Session.GetString("email");
            var user = await _unitOfWork.UserRepository.GetByEmail(email);

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
        /// <response code="409">If user with the same email is already exists</response>
        /// <response code="500">If unknown error occurred while creating</response>
        [HttpPost("Register"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromForm] EditUserDTO objectFromPage, IFormFile photo)
        {
            _logger.LogInformation($"POST {this}.Update called.");

            #region Validation.
            // Photo is an image type.
            if (photo != null && !photo.ContentType.Contains("image"))
            {
                _logger.LogError("Parameter photo is not an image");
                return BadRequest(new JsonResult(new { success = false, message = "File is not a photo" }));
            }
            // Valid model state.
            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is not valid");
                return BadRequest(ModelState);
            }
            // Users with the same email.
            var userFromDb = _unitOfWork.UserRepository.GetByEmail(objectFromPage.Email);
            if (userFromDb != null)
            {
                _logger.LogError("User with the same email is already exists");
                return Conflict();
            }
            #endregion
            // Building user.
            var user = _mapper.Map<User>(objectFromPage);
            user.ConfirmEmailToken = new ConfirmEmailToken
            {
                Token = _authService.GenerateRandomToken(),
                Expires = DateTime.Now.AddMinutes(5)
            };

            // Updating user.
            var result = await _userService.Create(user, photo);

            // Getting StatusCode of result.
            var StatusOfResult = (int)result
                .GetType()
                .GetProperty("StatusCode")
                .GetValue(result, null);

            // Checking StatusOfResult to send email.
            if (StatusOfResult == 201)
            {
                var IsEmailSend = await _authService.SendConfirmationEmail(user, Request, Url);

                if (IsEmailSend)
                    result = StatusCode(201, "Email was sended");
                else
                    result = StatusCode(201, "Email was not sended");
            }

            _logger.LogInformation($"POST {this}.Update finished.");

            return result;
        }

        /// <summary>
        /// Confirms email.
        /// </summary>
        /// <response code="200">If email successfuly confirmed</response>        
        /// <response code="400">If token is expired</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If cannot find user with current email</response>
        /// <response code="409">If cannot user is already confirmed</response>
        /// <response code="500">If unknown error occurred while creating</response>
        [HttpGet("Confirm"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string tokenValue, int userId)
        {
            _logger.LogInformation($"POST {this}.ConfirmEmail called.");

            // Getting TokenFromDb.
            var TokenFromDb = await _unitOfWork.ConfirmEmailTokenRepository.GetByToken(tokenValue);

            #region Validation.
            // Existing token.
            if (TokenFromDb == null || TokenFromDb.UserId != userId)
            {
                _logger.LogError($"Token not found.");
                return NotFound();
            }
            // Is user is already confirmed
            if (TokenFromDb.User.IsConfirmed)
            {
                _logger.LogError("Account is already confirmed");
                return Conflict();
            }
            // Is token already expired
            if (TokenFromDb.Expires < DateTime.Now)
            {
                _logger.LogError($"Token expired.");
                return StatusCode(498);
            }
            #endregion
            // Action.
            var result = await _authService.ConfirmEmail(TokenFromDb);

            _logger.LogInformation($"POST {this}.ConfirmEmail finished.");

            return result;
        }
        
        /// <summary>
        /// Confirms email.
        /// </summary>
        /// <response code="200">If email successfuly sended</response>        
        /// <response code="404">If cannot find user with current email</response>
        /// <response code="409">If user is already confirmed</response>
        /// <response code="500">If email was not send</response>
        [HttpPost("SendConfirmationEmail"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendConfirmationEmail([FromQuery] string email)
        {
            _logger.LogInformation($"POST {this}.SendConfirmationEmail called.");

            // Getting userFromDb.
            var userFromDb = await _unitOfWork.UserRepository.GetByEmail(email);

            #region Validation.
            // Existing user.
            if (userFromDb == null)
            {
                _logger.LogError($"User not found.");
                return NotFound();
            }
            // Is user is already confirmed
            if (userFromDb.IsConfirmed)
            {
                _logger.LogError("Account is already confirmed");
                return Conflict();
            }
            #endregion

            userFromDb.ConfirmEmailToken.Token = _authService.GenerateRandomToken();
            userFromDb.ConfirmEmailToken.Expires = DateTime.Now.AddMinutes(5);

            // Updating user.
            var result = await _userService.Update(userFromDb, null);

            // Getting StatusCode of result.
            var StatusOfResult = (int)result
                .GetType()
                .GetProperty("StatusCode")
                .GetValue(result, null);

            // Checking StatusOfResult to send email.
            if (StatusOfResult == 201)
            {
                var IsEmailSend = await _authService.SendConfirmationEmail(userFromDb, Request, Url);

                if (IsEmailSend)
                    result = Ok();
                else
                    result = StatusCode(500);
            }

            _logger.LogInformation($"POST {this}.SendConfirmationEmail finished.");

            return result;
        }

    }
}
