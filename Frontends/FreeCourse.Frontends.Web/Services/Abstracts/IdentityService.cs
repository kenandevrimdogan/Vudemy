using FreeCourse.Frontends.Web.Models;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Shared.Dtos.Response;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var disko = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disko.IsError)
            {
                throw disko.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disko.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                return null;
            }

            var authenticationTokens = (new List<AuthenticationToken>
            {
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.AccessToken, Value= token.AccessToken},
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.RefreshToken, Value= token.RefreshToken},
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.ExpiresIn, Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O", CultureInfo.InvariantCulture)},
            });

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;

            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var disko = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disko.IsError)
            {
                throw disko.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            if(refreshToken != null)
            {
                TokenRevocationRequest tokenRevocationRequest = new TokenRevocationRequest
                {
                    ClientId = _clientSettings.WebClientForUser.ClientId,
                    ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                    Address = disko.RevocationEndpoint,
                    Token = refreshToken,
                    TokenTypeHint = "refresh_token"
                };

                await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
            }
        }

        public async Task<ResponseDTO<bool>> SignIn(SignInInput signInInput)
        {
            var disko = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disko.IsError)
            {
                throw disko.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = disko.TokenEndpoint,
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDTO = JsonSerializer.Deserialize<ErrorDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return ResponseDTO<bool>.Fail(errorDTO.Errors, HttpStatusCode.BadRequest);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disko.UserInfoEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.AccessToken, Value= token.AccessToken},
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.RefreshToken, Value= token.RefreshToken},
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.ExpiresIn, Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O", CultureInfo.InvariantCulture)},
            });

            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return ResponseDTO<bool>.Success(HttpStatusCode.OK);
        }
    }
}
