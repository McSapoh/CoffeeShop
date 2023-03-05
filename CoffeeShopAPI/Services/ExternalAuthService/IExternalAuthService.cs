using CoffeeShopAPI.Helpers.DTO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IExternalAuthService
    {
        public string GetAuthorizeUrl(ExternalAuthorizationRequest parameters);
        public Task<string> ExchangeCodeForToken(CodeForTokenRequest parameters);
        public Task<string> GetUserInfo(string url, string accessToken);
    }
}
