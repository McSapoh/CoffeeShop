using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IImagesService
    {
        public Task<string> SavePhoto(IFormFile photo);
        public void DeletePhoto(string path);
    }
}
