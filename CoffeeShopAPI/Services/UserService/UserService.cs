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
    public class UserService : ControllerBase, IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IImagesService _imagesService;

        public UserService(IConfiguration config, IUnitOfWork unitOfWork, 
            ILogger<UserService> logger, IImagesService imagesService)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imagesService = imagesService;
        }

        public async Task<IActionResult> Update(User user, IFormFile photo)
        {
            // Checking existing object.
            var objectFromDb = _unitOfWork.UserRepository.GetById(user.Id);
            if (objectFromDb == null)
            {
                _logger.LogError($"Cannot find object with id = {user.Id}");
                return NotFound(new JsonResult(new
                {
                    success = false,
                    message = $"Cannot find user with id {user.Id}"
                }));
            }

            // Checking existing user with same email.
            if (user.Email != objectFromDb.Email)
            {
                var checkEmailUser = await _unitOfWork.UserRepository.GetByEmail(user.Email);
                if (checkEmailUser != null && checkEmailUser.Email == user.Email)
                {
                    _logger.LogError("User with this email is already exists");
                    return Conflict(new JsonResult(new
                    {
                        success = false,
                        message = "User with this email is already exists"
                    }));
                }
            }

            // Updating user.
            objectFromDb.Adress = user.Adress;
            objectFromDb.Email = user.Email;
            objectFromDb.Name = user.Name;
            objectFromDb.Password = user.Password;

            // Updating user.
            _unitOfWork.UserRepository.Update(objectFromDb);
            if (!await _unitOfWork.SaveAsync())
                return BadRequest(new JsonResult(new
                {
                    success = false,
                    message = "Error while updating, photo has not been saved"
                }));


            // Saving photo and and updating user.ImagePath.
            // Saving photo.
            var imagePath = await _imagesService.SavePhoto("User", photo);

            // Chaecking is photo were saved.
            if (imagePath != null)
            {
                _logger.LogInformation("Photo has been saved");
                objectFromDb.ImagePath = imagePath;
                // Saving user with new ImagePath.
                _unitOfWork.UserRepository.Update(objectFromDb);
                var savingresult = await _unitOfWork.SaveAsync();

                if (savingresult)
                    return Ok(new JsonResult(new
                    {
                        success = true,
                        message = "Successfully updated"
                    }));
                else
                    return BadRequest(new JsonResult(new
                    {
                        success = false,
                        message = "Error while updating, Photo has been saved"
                    }));
            }

            return Ok(new JsonResult(new
            {
                success = true,
                message = "Successfully updated"
            }));
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
            string token;

            // Creating base64 encoded random security token.
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                token = Convert.ToBase64String(tokenData);
            }

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

        public void AppendRefreshTokenToResponse(RefreshToken newRefreshToken)
        {
            _logger.LogInformation($"{this}.AppendRefreshTokenToResponse called.");

            // Setting cookies.
            var cookieOptions = new CookieOptions { 
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            // Appending data to response.
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            _logger.LogInformation($"{this}.AppendRefreshTokenToResponse finished.");
        }
    }
}
