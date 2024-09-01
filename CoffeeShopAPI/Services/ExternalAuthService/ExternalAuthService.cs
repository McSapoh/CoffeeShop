using CoffeeShopAPI.Controllers;
using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class ExternalAuthService : ControllerBase, IExternalAuthService
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<ExternalAuthController> _logger;
		private readonly IUserService _userService;
		private readonly IAuthService _authService;
		private readonly IConfiguration _config;

		public ExternalAuthService(IUnitOfWork unitOfWork, ILogger<ExternalAuthController> logger, IUserService userService, IAuthService authService, IConfiguration config)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_userService = userService;
			_authService = authService;
			_config = config;
		}

		public string GetAuthorizeUrl (ExternalAuthorizationRequest parameters)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", parameters.ClientId},
                { "redirect_uri", parameters.RedirectUrl },
                { "response_type", "code" },
                { "scope", parameters.Scope },
                { "code_challenge", parameters.CodeChallenge  },
                { "code_challenge_method", "S256" },
                { "access_type", "offline" }
            };
            var url = QueryHelpers.AddQueryString(parameters.Uri, queryParams);
            return url;
        }

        public async Task<string> ExchangeCodeForToken(CodeForTokenRequest parameters)
        {
            var httpClient = new HttpClient();

            var tokenRequestParams = new Dictionary<string, string>
            {
                { "client_id", parameters.ClientId },
                { "client_secret", parameters.ClientSecret },
                { "code", parameters.Code },
                { "code_verifier", parameters.CodeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", parameters.RedirectUrl }
            };

            var tokenRequestContent = new FormUrlEncodedContent(tokenRequestParams);

            // Send the token request to the Google OAuth2 API.
            var tokenRequestResult = await httpClient.PostAsync(
                parameters.PostUrl, tokenRequestContent
            );

            // Check if the token request was successful.
            if (!tokenRequestResult.IsSuccessStatusCode)
            {
                // If the token request failed, return a BadRequest result.
                return null;
            }

            // If the token request was successful, parse the JSON response and get the access token.
            var tokenResponse = await tokenRequestResult.Content.ReadAsStringAsync();
            var tokenObj = JObject.Parse(tokenResponse);

            var accessToken = (string)tokenObj["access_token"];
            return accessToken;
        }
        public async Task<string> GetUserInfo(string url, string accessToken)
        {
            var httpClient = new HttpClient();
            // Create a request to get the user info.
            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, url);

            // Set the Authorization header to include the access token.
            userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Send the user info request to the Google OAuth2 API.
            var userInfoResult = await httpClient.SendAsync(userInfoRequest);

            // Check if the user info request was successful.
            if (!userInfoResult.IsSuccessStatusCode)
            {
                // If the user info request failed, return a BadRequest result.
                return null;
            }

            // If the user info request was successful, parse the JSON response and get the user's email.
            var userInfoResponse = await userInfoResult.Content.ReadAsStringAsync();
            return userInfoResponse;
        }

		public async Task<IActionResult> GoogleSignIn(string code, string codeVerifier, HttpResponse Response)
		{
			#region Getting accessToken.
			// Initialize variables to store the access token and JWT token.
			string JWTToken = "";

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
			var accessToken = await ExchangeCodeForToken(parameters);

			// Validating accessToken.
			if (string.IsNullOrEmpty(accessToken))
			{
				_logger.LogInformation($"Failed to retrieve access token.");
				return Unauthorized();
			}
			#endregion
			#region Getting userInfo.
			// Get the user info using the access token and the Google OAuth2 API.
			var userInfoResponse = await GetUserInfo(
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
				if (!userFromDb.IsConfirmed)
				{
					userFromDb.IsConfirmed = true;

					// Saving updated user information into the database.
					_unitOfWork.UserRepository.Update(userFromDb);

					var savingresult = await _unitOfWork.SaveAsync();
				}
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
				}
			}
			#endregion
			_logger.LogInformation($"GET {this}.GoogleSignIn finished.");

			string appRedirectUrl = _config.GetValue<string>("FrontAppUrlExternalAuthRedirect") + JWTToken;
			return Redirect(appRedirectUrl);
		}
		public async Task<IActionResult> FacebookSignIn(string code, string codeVerifier, HttpResponse Response)
		{
			_logger.LogInformation($"GET {this}.FacebookSignIn called.");

			#region Getting accessToken.

			// Initialize variables to store the access token and JWT token.
			string JWTToken = "";

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
			var accessToken = await ExchangeCodeForToken(parameters);

			// Validating accessToken.
			if (string.IsNullOrEmpty(accessToken))
			{
				_logger.LogInformation($"Failed to retrieve access token.");
				return Unauthorized();
			}
			#endregion
			#region Getting userInfo.
			// Get the user info using the access token and the Google OAuth2 API.
			var userInfoResponse = await GetUserInfo(
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
				if (!userFromDb.IsConfirmed)
				{
					userFromDb.IsConfirmed = true;

					// Saving updated user information into the database.
					_unitOfWork.UserRepository.Update(userFromDb);

					var savingresult = await _unitOfWork.SaveAsync();
				}
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
				}
			}
			#endregion
			_logger.LogInformation($"GET {this}.GoogleSignIn finished.");

			string appRedirectUrl = _config.GetValue<string>("FrontAppUrlExternalAuthRedirect") + JWTToken;
			return Redirect(appRedirectUrl);
		}
	}
}
