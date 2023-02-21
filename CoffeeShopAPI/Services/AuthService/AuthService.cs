using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class AuthService : ControllerBase, IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration config, IUnitOfWork unitOfWork, 
            ILogger<AuthService> logger)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<User> GetUserByIdentity(HttpContext context)
        {
            _logger.LogInformation($"{this}.GetUser called.");

            // Getting identity from HttpContext.
            var identity = context.User.Identity as ClaimsIdentity;

            // Getting user by claims
            var user = await _unitOfWork.UserRepository.GetByEmail(
                identity.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Email).Value
            );

            _logger.LogInformation($"{this}.GetUser finished with result {user}.");
            return user;
        }

        public string GenerateJWT(User user)
        {
            _logger.LogInformation($"{this}.GenerateJWT called.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Key"]));
            var credentias = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Building Claims array
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Building JwtSecurityToken.
            var token = new JwtSecurityToken(
                _config["Auth:Issuer"],
                _config["Auth:Audience"],
                claims,
                expires: DateTime.Now.AddHours(Convert.ToDouble(_config["Auth:JWTExpires"])),
                signingCredentials: credentias
            );

            _logger.LogInformation($"{this}.GenerateJWT JwtsevurityToken was build.");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            _logger.LogInformation($"{this}.GenerateRefreshToken called.");
            string token = GenerateRandomToken();

            // Building RefreshToken.
            var refreshToken = new RefreshToken
            {
                Token = token,
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(double.Parse(_config["Auth:RefreshExpires"])),
            };

            _logger.LogInformation($"{this}.GenerateRefreshToken finished.");
            return refreshToken;
        }

        public string GenerateRandomToken()
        {
            // Creating base64 encoded random security token.
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                var token = Convert.ToBase64String(tokenData);
                return token;
            }
        }
        public void AppendRefreshTokenToResponse(RefreshToken newRefreshToken, HttpResponse response)
        {
            _logger.LogInformation($"{this}.AppendRefreshTokenToResponse called.");

            // Setting cookies.
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            // Appending data to response.
            response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            _logger.LogInformation($"{this}.AppendRefreshTokenToResponse finished.");
        }
        
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailToken confirmEmailToken)
        {
            // Confirmation email.
            confirmEmailToken.User.IsConfirmed = true;

            // Saving changes.
            if (!await _unitOfWork.SaveAsync())
            {
                _logger.LogError("Unknown error occurred while creating");
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
