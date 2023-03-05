using CoffeeShopAPI.Helpers.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class ExternalAuthService : ControllerBase, IExternalAuthService
    {
        public string GetAuthorizeUrl (ExternalAuthorizationRequest parameters)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", parameters.ClientId},
                { "redirect_uri", parameters.RedirectUrl },
                { "response_type", "code" },
                { "scope", parameters.Scope },
                { "code_challenge", parameters.CodeChallenge  },
                { "code_challenge_method", "S256" },
                { "access_type", "offline" }
            };
            var url = QueryHelpers.AddQueryString(parameters.Uri, queryParams);
            return url;
        }

        public async Task<string> ExchangeCodeForToken(CodeForTokenRequest parameters)
        {
            var httpClient = new HttpClient();

            var tokenRequestParams = new Dictionary<string, string>
            {
                { "client_id", parameters.ClientId },
                { "client_secret", parameters.ClientSecret },
                { "code", parameters.Code },
                { "code_verifier", parameters.CodeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", parameters.RedirectUrl }
            };

            var tokenRequestContent = new FormUrlEncodedContent(tokenRequestParams);

            // Send the token request to the Google OAuth2 API.
            var tokenRequestResult = await httpClient.PostAsync(
                parameters.PostUrl, tokenRequestContent
            );

            // Check if the token request was successful.
            if (!tokenRequestResult.IsSuccessStatusCode)
            {
                // If the token request failed, return a BadRequest result.
                return null;
            }

            // If the token request was successful, parse the JSON response and get the access token.
            var tokenResponse = await tokenRequestResult.Content.ReadAsStringAsync();
            var tokenObj = JObject.Parse(tokenResponse);

            var accessToken = (string)tokenObj["access_token"];
            return accessToken;
        }
        public async Task<string> GetUserInfo(string url, string accessToken)
        {
            var httpClient = new HttpClient();
            // Create a request to get the user info.
            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, url);

            // Set the Authorization header to include the access token.
            userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Send the user info request to the Google OAuth2 API.
            var userInfoResult = await httpClient.SendAsync(userInfoRequest);

            // Check if the user info request was successful.
            if (!userInfoResult.IsSuccessStatusCode)
            {
                // If the user info request failed, return a BadRequest result.
                return null;
            }

            // If the user info request was successful, parse the JSON response and get the user's email.
            var userInfoResponse = await userInfoResult.Content.ReadAsStringAsync();
            return userInfoResponse;
        }
    }
}
