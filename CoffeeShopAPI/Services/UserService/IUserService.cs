﻿using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IUserService
    {
        public Task<IActionResult> Update(User user, IFormFile photo);
        public Task<User> GetUserByIdentity();
        public string GenerateJWT(User user);
        public RefreshToken GenerateRefreshToken();
        public void AppendRefreshTokenToResponse(RefreshToken newRefreshToken);
    }
}