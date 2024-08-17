using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IConfiguration _config;
        public ImagesService(IConfiguration _config) 
        {
            this._config = _config;
        }
        public async Task<string> SavePhoto(IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                try
                {
                    #region Getting random string
                    var random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var randomSting = new string(Enumerable.Repeat(chars, 12)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                    #endregion
                    var fileName = randomSting + Path.GetExtension(photo.FileName);

                    string configPath = _config.GetValue<string>("ImagesDirPath");
                    string filePath = "";
                    if (string.IsNullOrWhiteSpace(configPath))
                        throw new Exception("Cannot find ImagesDirPath");

                    filePath = Path.Combine(configPath, fileName);
                    
                    using Stream fileStream = new FileStream(filePath, FileMode.Create);
                    await photo.CopyToAsync(fileStream);
                    return fileName;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return null;
        }
        public void DeletePhoto(string path)
        {
            try
            {
                var directoryPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                var fullPath = Path.Combine(directoryPath + "\\Images\\" + path);
                File.Delete(fullPath);
            }
            catch (Exception)
            { }
        }
    }
}
