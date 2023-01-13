﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IImagesService
    {
        public Task<string> SavePhoto(string path, IFormFile photo);
        public bool DeletePhoto(string path);
    }
}