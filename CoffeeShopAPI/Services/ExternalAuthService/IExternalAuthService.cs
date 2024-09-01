using CoffeeShopAPI.Helpers.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IExternalAuthService
    {
        public string GetAuthorizeUrl(ExternalAuthorizationRequest parameters);
        public Task<string> ExchangeCodeForToken(CodeForTokenRequest parameters);
        public Task<string> GetUserInfo(string url, string accessToken);
        public Task<IActionResult> FacebookSignIn(string code, string codeVerifier, HttpResponse Response);
        public Task<IActionResult> GoogleSignIn(string code, string codeVerifier, HttpResponse Response);

	}
}
