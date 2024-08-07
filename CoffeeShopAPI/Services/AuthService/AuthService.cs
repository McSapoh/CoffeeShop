﻿using CoffeeShopAPI.Models;
using CoffeeShopAPI.UnitOfWork;
using IdentityModel;
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
        private readonly IEmailService _emailService;

        public AuthService(IConfiguration config, IUnitOfWork unitOfWork, 
            ILogger<AuthService> logger, IEmailService emailService)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailService = emailService;
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
                new Claim("name", user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("profileImagePath", user.ImagePath ?? ""),
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

                var token = Convert.ToBase64String(tokenData)
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .Replace("=", "");
                return token;
            }
        }

        public string GenerateRandomToken(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var tokenData = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var token = Convert.ToBase64String(tokenData)
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .Replace("=", "");
            return token;
        }

        public void AppendRefreshTokenToResponse(RefreshToken newRefreshToken, HttpResponse response)
        {
            _logger.LogInformation($"{this}.AppendRefreshTokenToResponse called.");

            // Setting cookies.
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
                SameSite = SameSiteMode.None,
                Secure = true
            };

            // Appending data to response.
            response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            _logger.LogInformation($"{this}.AppendRefreshTokenToResponse finished.");
        }
        
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailToken confirmEmailToken)
        {
            // Confirmation email.
            confirmEmailToken.User.IsConfirmed = true;

            // Expiring token.
            confirmEmailToken.Expires = DateTime.Now;

            // Saving changes.
            if (!await _unitOfWork.SaveAsync())
            {
                _logger.LogError("Unknown error occurred while creating");
                return StatusCode(500);
            }
            return Ok();
        }

        public async Task<bool> SendConfirmationEmail(User userFromDb, HttpRequest request, IUrlHelper url)
        {
            // Building email.
            var ConfirmationLink = request.Scheme + "://" + request.Host +
                url.Action("ConfirmEmail", "Auth") +
                $"?tokenValue={userFromDb.ConfirmEmailToken.Token}&userId={userFromDb.Id}";
            var Email = _emailService.BuildConfirmationMail(
                userFromDb.Email, userFromDb.Name, ConfirmationLink
            );

            // Sending Email.
            return await _emailService.SendEmail(Email);
        }
    }
}
