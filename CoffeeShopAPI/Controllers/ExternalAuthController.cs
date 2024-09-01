using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
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

		/// <summary>
		/// Gets data from external auth server and creates or updates user.
		/// </summary>
		/// <response code="200">Successfully signed in, returns JWT</response>
		/// <response code="401">Failed to retrieve access token or user information</response>
		/// <response code="500">If unknown error occurred while creating or updating</response>
		[HttpGet("{provider}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ExternalAuthCallback(string provider, [FromQuery(Name = "code")] string code)
		{
			_logger.LogInformation($"GET {this}.FacebookSignIn called.");

			var codeVerifier = HttpContext.Session.GetString("codeVerifier");
			Task<IActionResult> result = null;
			switch (provider)
			{
				case "facebook":
					result = _externalAuthService.FacebookSignIn(code, codeVerifier, Response);
					break;
				case "google":
					result = _externalAuthService.GoogleSignIn(code, codeVerifier, Response);
					break;
				default:
					break;
			}

			_logger.LogInformation($"GET {this}.ExternalAuthCallback finished.");
			return result.Result;
		}
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
            var codeVerifier = GenerateCodeVerifier();

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

		private static string GenerateCodeVerifier()
		{
			const int length = 64; // You can choose a length between 43 and 128
			using (var rng = new RNGCryptoServiceProvider())
			{
				var randomBytes = new byte[length];
				rng.GetBytes(randomBytes);

				// Convert to a base64 string, removing non-PKCE compliant characters
				var codeVerifier = Convert.ToBase64String(randomBytes)
					.Replace('+', '-')
					.Replace('/', '_')
					.Replace("=", string.Empty); // "=" padding must be removed for PKCE

				return codeVerifier.Substring(0, length); // Ensure length is exactly 64 characters
			}
		}
	}
}
