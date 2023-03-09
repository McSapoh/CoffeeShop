using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    public class ExternalAuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExternalAuthController> _logger;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly IExternalAuthService _externalAuthService;

        public ExternalAuthController(IUnitOfWork unitOfWork, IAuthService authService,
            ILogger<ExternalAuthController> logger, IUserService userService, IConfiguration configuration,
            IExternalAuthService externalAuthService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
            _authService = authService;
            _config = configuration;
            _externalAuthService = externalAuthService;
        }

        #region Facebook
        /// <summary>
        /// Gets data from external auth server and creates or updates user.
        /// </summary>
        /// <response code="200">Successfully signed in, returns JWT</response>
        /// <response code="401">Failed to retrieve access token or user information</response>
        /// <response code="500">If unknown error occurred while creating or updating</response>
        [HttpGet("facebook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FacebookSignIn([FromQuery(Name = "code")] string code)
        {
            _logger.LogInformation($"GET {this}.FacebookSignIn called.");

            #region Getting accessToken.
            // Get the code verifier from the session.
            var codeVerifier = HttpContext.Session.GetString("codeVerifier");

            // Initialize variables to store the access token and JWT token.
            string JWTToken;

            // Create a CodeForTokenRequest object with the necessary parameters.
            var parameters = new CodeForTokenRequest
            {
                ClientId = _config["Auth:Facebook:AppId"],
                ClientSecret = _config["Auth:Facebook:AppSecret"],
                Code = code,
                RedirectUrl = "https://localhost:44300/api/ExternalAuth/facebook",
                CodeVerifier = codeVerifier,
                PostUrl = "https://graph.facebook.com/v13.0/oauth/access_token"
            };

            // Exchanging code to access token.
            var accessToken = await _externalAuthService.ExchangeCodeForToken(parameters);

            // Validating accessToken.
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogInformation($"Failed to retrieve access token.");
                return Unauthorized();
            }
            #endregion
            #region Getting userInfo.
            // Get the user info using the access token and the Google OAuth2 API.
            var userInfoResponse = await _externalAuthService.GetUserInfo(
                "https://graph.facebook.com/me?fields=id,name,email,picture.type(large)", accessToken
            );

            // Validating userInfoResponse.
            if (string.IsNullOrEmpty(userInfoResponse))
            {
                _logger.LogInformation($"Failed to retrieve user info.");
                return Unauthorized();
            }

            var userInfoObj = JObject.Parse(userInfoResponse);
            #endregion

            var email = (string)userInfoObj["email"];

            // Check if the user is already registered in the database.
            var userFromDb = await _unitOfWork.UserRepository.GetByEmail(email);

            #region Updating userFromDb.
            if (userFromDb != null)
            {
                // Generating JWT token for the user.
                JWTToken = _authService.GenerateJWT(userFromDb);

                // Appending RefreshToken to the response.
                _authService.AppendRefreshTokenToResponse(userFromDb.RefreshToken, Response);

                // Confirming email if necessary.
                if (userFromDb.IsConfirmed)
                {
                    _logger.LogInformation($"GET {this}.GoogleSignIn finished with result {true}.");
                    return Ok(JWTToken);
                }

                userFromDb.IsConfirmed = true;

                // Saving updated user information into the database.
                _unitOfWork.UserRepository.Update(userFromDb);

                var savingresult = await _unitOfWork.SaveAsync();

                _logger.LogInformation($"GET {this}.GoogleSignIn finished with result {savingresult}.");
                if (savingresult)
                    return Ok(JWTToken);
                else
                    return StatusCode(500);
            }
            #endregion
            #region Creating userFromDb.
            else
            {
                // Creating a new user with retrieved user information.
                var user = new User
                {
                    Name = (string)userInfoObj["name"],
                    Email = email,
                    IsConfirmed = true,
                    ImagePath = (string)userInfoObj["picture"]
                };

                var result = await _userService.Create(user, null);

                // Getting StatusCode of result.
                var StatusOfResult = (int)result
                    .GetType()
                    .GetProperty("StatusCode")
                    .GetValue(result, null);

                // Checking StatusOfResult to generate JWTToken.
                if (StatusOfResult == 201)
                {
                    // Getting tokens.
                    JWTToken = _authService.GenerateJWT(user);

                    // Appending RefreshToken to response.
                    _authService.AppendRefreshTokenToResponse(user.RefreshToken, Response);

                    result = Ok(JWTToken);
                }

                _logger.LogInformation($"GET {this}.GoogleSignIn finished.");
                return result;
            }
            #endregion
        }
        #endregion
        #region Google.
        /// <summary>
        /// Gets data from external auth server and creates or updates user.
        /// </summary>
        /// <response code="200">Successfully signed in, returns JWT</response>
        /// <response code="401">Failed to retrieve access token or user information</response>
        /// <response code="500">If unknown error occurred while creating or updating</response>
        [HttpGet("google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GoogleSignIn([FromQuery(Name = "code")] string code)
        {
            _logger.LogInformation($"GET {this}.GoogleSignIn called.");

            #region Getting accessToken.
            // Get the code verifier from the session.
            var codeVerifier = HttpContext.Session.GetString("codeVerifier");

            // Initialize variables to store the access token and JWT token.
            string JWTToken;

            // Create a `CodeForTokenRequest` object with the necessary parameters.
            var parameters = new CodeForTokenRequest
            {
                ClientId = _config["Auth:Google:ClientId"],
                ClientSecret = _config["Auth:Google:ClientSecret"],
                Code = code,
                RedirectUrl = "https://localhost:44300/api/ExternalAuth/google",
                CodeVerifier = codeVerifier,
                PostUrl = "https://oauth2.googleapis.com/token"
            };

            // Exchnging cpde to access token.
            var accessToken = await _externalAuthService.ExchangeCodeForToken(parameters);

            // Validating accessToken.
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogInformation($"Failed to retrieve access token.");
                return Unauthorized();
            }
            #endregion
            #region Getting userInfo.
            // Get the user info using the access token and the Google OAuth2 API.
            var userInfoResponse = await _externalAuthService.GetUserInfo(
                "https://www.googleapis.com/oauth2/v1/userinfo", accessToken
            );

            // Validating userInfoResponse.
            if (string.IsNullOrEmpty(userInfoResponse))
            {
                _logger.LogInformation($"Failed to retrieve user info.");
                return Unauthorized();
            }

            var userInfoObj = JObject.Parse(userInfoResponse);
            #endregion

            var email = (string)userInfoObj["email"];

            // Check if the user is already registered in the database.
            var userFromDb = await _unitOfWork.UserRepository.GetByEmail(email);

            #region Updating userFromDb.
            if (userFromDb != null)
            {
                // Generating JWT token for the user.
                JWTToken = _authService.GenerateJWT(userFromDb);

                // Appending RefreshToken to the response.
                _authService.AppendRefreshTokenToResponse(userFromDb.RefreshToken, Response);

                // Confirming email if necessary.
                if (userFromDb.IsConfirmed)
                {
                    _logger.LogInformation($"GET {this}.GoogleSignIn finished with result {true}.");
                    return Ok(JWTToken);
                }
                
                userFromDb.IsConfirmed = true;

                // Saving updated user information into the database.
                _unitOfWork.UserRepository.Update(userFromDb);

                var savingresult = await _unitOfWork.SaveAsync();

                _logger.LogInformation($"GET {this}.GoogleSignIn finished with result {savingresult}.");
                if (savingresult)
                    return Ok(JWTToken);
                else
                    return StatusCode(500);
            }
            #endregion
            #region Creating userFromDb.
            else
            {
                // Creating a new user with retrieved user information.
                var user = new User
                {
                    Name = (string)userInfoObj["name"],
                    Email = email,
                    IsConfirmed = true,
                    ImagePath = (string)userInfoObj["picture"]
                };

                var result = await _userService.Create(user, null);
                
                // Getting StatusCode of result.
                var StatusOfResult = (int)result
                    .GetType()
                    .GetProperty("StatusCode")
                    .GetValue(result, null);

                // Checking StatusOfResult to generate JWTToken.
                if (StatusOfResult == 201)
                {
                    // Getting tokens.
                    JWTToken = _authService.GenerateJWT(user);

                    // Appending RefreshToken to response.
                    _authService.AppendRefreshTokenToResponse(user.RefreshToken, Response);

                    result =  Ok(JWTToken);
                }

                _logger.LogInformation($"GET {this}.GoogleSignIn finished.");
                return result;
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// Sends auth request to external auth server.
        /// </summary>
        /// <response code="308">Redirecting to external auth provider</response>
        /// <response code="500">Unknown external auth provider</response>
        [HttpGet("signin-by-{provider}")]
        [ProducesResponseType(StatusCodes.Status308PermanentRedirect)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SignIn(string provider)
        {
            _logger.LogInformation($"GET {this}.SignIn called.");

            // Generating a random code verifier and store it in the session.
            var codeVerifier = Guid.NewGuid().ToString();

            // Saving codeVerifier to session.
            HttpContext.Session.SetString("codeVerifier", codeVerifier);

            // Generating a code challenge based on the code verifier.
            var codeChallenge = _authService.GenerateRandomToken(codeVerifier);

            // Building the parameters for the authorization requests.
            ExternalAuthorizationRequest parameters = null;
            switch (provider.ToLower())
            {
                case "google":
                    parameters = new ExternalAuthorizationRequest
                    {
                        ClientId = _config["Auth:Google:ClientId"],
                        RedirectUrl = "https://localhost:44300/api/ExternalAuth/google",
                        Scope = "profile email",
                        CodeChallenge = codeChallenge,
                        Uri = "https://accounts.google.com/o/oauth2/v2/auth"
                    };
                    break;
                case "facebook":
                    parameters = new ExternalAuthorizationRequest
                    {
                        ClientId = _config["Auth:Facebook:AppId"],
                        RedirectUrl = "https://localhost:44300/api/ExternalAuth/facebook",
                        Scope = "public_profile email",
                        CodeChallenge = codeChallenge,
                        Uri = "https://www.facebook.com/v13.0/dialog/oauth"
                    };
                    break;
                default:
                    return StatusCode(500);
            }

            
            // Generating the authorization URL for the request.
            var url = _externalAuthService.GetAuthorizeUrl(parameters);

            _logger.LogInformation($"GET {this}.SignIn finished, redirecting.");

            // Redirecting the user to the External authorization endpoint.
            return Redirect(url);
        }
    }
}
