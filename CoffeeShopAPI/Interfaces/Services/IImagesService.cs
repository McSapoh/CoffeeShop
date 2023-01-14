using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Services
{
    public interface IImagesService
    {
        public Task<string> SavePhoto(string type, IFormFile photo);
        public void DeletePhoto(string path);
    }
}
