using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IAuthService
    {
        public Task<User> GetUserByIdentity(HttpContext context);
        public string GenerateJWT(User user);
        public RefreshToken GenerateRefreshToken();
        public void AppendRefreshTokenToResponse(RefreshToken newRefreshToken);
    }
}
