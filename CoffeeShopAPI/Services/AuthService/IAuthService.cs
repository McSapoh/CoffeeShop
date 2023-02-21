using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IAuthService
    {
        public Task<User> GetUserByIdentity(HttpContext context);
        public string GenerateJWT(User user);
        public RefreshToken GenerateRefreshToken();
        public string GenerateRandomToken();
        public void AppendRefreshTokenToResponse(RefreshToken newRefreshToken, HttpResponse response);
        public Task<IActionResult> ConfirmEmail(ConfirmEmailToken confirmEmailToken);
        public Task<bool> SendConfirmationEmail(User userFromDb);
    }
}
