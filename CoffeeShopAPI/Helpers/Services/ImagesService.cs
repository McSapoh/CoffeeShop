using CoffeeShopAPI.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Helpers.Services
{
    public class ImagesService : IImagesService
    {
        public async Task<bool> SavePhoto(string path, IFormFile photo)
        {
            try
            {
                #region Getting random string
                var random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var randomSting =  new string(Enumerable.Repeat(chars, 12)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                #endregion
                var fileName = randomSting + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(path, fileName);
                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(fileStream);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeletePhoto(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
