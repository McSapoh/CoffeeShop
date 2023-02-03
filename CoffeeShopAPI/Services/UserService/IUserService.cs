using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services.UserService
{
    public interface IUserService
    {
        public Task<User> GetUserByIdentity();
        public string GenerateJWT(User user);
        public RefreshToken GenerateRefreshToken();
        public void AppendRefreshTokenToResponse(RefreshToken newRefreshToken);
    }
}
