using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using CulinaryBlogCore.Services.Utils;

using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Imgur.API.Models;
using Microsoft.Extensions.Options;

namespace CulinaryBlogCore.Services.Services
{
    public class ImgurTokenService : IImgurTokenService
    {
        private readonly IRepository _repository;
        private readonly ImgurConfigData _imgurConfigData;

        public ImgurTokenService(
            IRepository repository,
            IOptions<ImgurConfigData> imgurConfigData)
        {
            this._repository = repository;
            this._imgurConfigData = imgurConfigData.Value;
        }

        public async Task<ImageEndpoint> GetImageEndpoint()
        {
            string clientId = this._imgurConfigData.ClientId;
            string clientSecret = this._imgurConfigData.ClientSecret;
            var apiClient = new ApiClient(clientId, clientSecret);
            var httpClient = new HttpClient();

            OAuth2Token token = await this.GetLastToken(apiClient, httpClient);
            apiClient.SetOAuth2Token(token);
            var imageEndpoint = new ImageEndpoint(apiClient, httpClient);
            return imageEndpoint;
        }

        private async Task<OAuth2Token> GetLastToken(ApiClient apiClient, HttpClient httpClient)
        {
            ImgurToken imgurToken = this._repository.Set<ImgurToken>()
                .Where(t => DateTime.Now <= t.CreationTime.AddSeconds(t.ExpiresIn))
                .OrderByDescending(t => t.CreationTime)
                .FirstOrDefault();

            var token = new OAuth2Token
            {
                RefreshToken = this._imgurConfigData.RefreshToken,
                AccountId = this._imgurConfigData.AccountId,
                AccountUsername = this._imgurConfigData.AccountUsername,
                TokenType = this._imgurConfigData.TokenType
            };

            if (imgurToken == null)
            {
                imgurToken = await this.CreateNewToken(apiClient, httpClient);
            }

            token.AccessToken = imgurToken.Token;
            token.ExpiresIn = imgurToken.ExpiresIn;
            return token;
        }

        private async Task<ImgurToken> CreateNewToken(ApiClient apiClient, HttpClient httpClient)
        {

            OAuth2Endpoint oAuth2Endpoint = new OAuth2Endpoint(apiClient, httpClient);
            // Generate url token: oAuth2Endpoint.GetAuthorizationUrl();
            var token = await oAuth2Endpoint.GetTokenAsync(this._imgurConfigData.RefreshToken);

            ImgurToken imgurToken = new ImgurToken
            {
                Token = token.AccessToken,
                ExpiresIn = token.ExpiresIn,
                CreationTime = DateTime.Now,
            };
            this._repository.Add<ImgurToken>(imgurToken);
            return imgurToken;
        }
    }
}
