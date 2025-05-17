using Blazored.LocalStorage;
using EventBookingSystem.Core.DTOs.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventBookingSystem.Webpage.Services
{
    public interface IClientAuthService
    {
        Task<AuthResponse> LoginAsync(LoginReq loginRequest);
        Task<AuthResponse> RegisterAsync(RegisterReq registerRequest);
        Task LogoutAsync();
        Task<AuthResponse> RefreshTokenAsync();
        Task<bool> IsUserAuthenticated();
    }

    public class ClientAuthService : IClientAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IJSRuntime _jsRuntime;

        public ClientAuthService(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage,
            IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _jsRuntime = jsRuntime;
        }

        public async Task<AuthResponse> LoginAsync(LoginReq loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResult.Success && !string.IsNullOrEmpty(authResult.Token))
            {
                await _localStorage.SetItemAsync("authToken", authResult.Token);
                await _jsRuntime.InvokeVoidAsync("document.cookie", $"refreshToken={authResult.RefreshToken}; path=/; secure; HttpOnly;");
                await _localStorage.SetItemAsync("tokenExpiration", authResult.AccessTokenExpiration);

                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
            }

            return authResult;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterReq registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResult.Success && !string.IsNullOrEmpty(authResult.Token))
            {
                await _localStorage.SetItemAsync("authToken", authResult.Token);
                await _jsRuntime.InvokeVoidAsync("document.cookie", $"refreshToken={authResult.RefreshToken}; path=/; secure; HttpOnly;");
                await _localStorage.SetItemAsync("tokenExpiration", authResult.AccessTokenExpiration);

                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
            }

            return authResult;
        }

        public async Task LogoutAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    await _httpClient.PostAsync("api/auth/logout", null);
                }
                catch
                {
                    // If server logout fails, continue with local logout
                }
            }

            await _localStorage.RemoveItemAsync("authToken");
            await _jsRuntime.InvokeVoidAsync("document.cookie", "refreshToken=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT;");
            await _localStorage.RemoveItemAsync("tokenExpiration");

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<AuthResponse> RefreshTokenAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _jsRuntime.InvokeAsync<string>("eval", "document.cookie.split('; ').find(row => row.startsWith('refreshToken=')).split('=')[1]");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                return new AuthResponse { Success = false, Error = "No token available" };
            }

            var refreshRequest = new RefreshTokenReq
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", refreshRequest);
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResult.Success && !string.IsNullOrEmpty(authResult.Token))
            {
                await _localStorage.SetItemAsync("authToken", authResult.Token);
                await _jsRuntime.InvokeVoidAsync("document.cookie", $"refreshToken={authResult.RefreshToken}; path=/; secure; HttpOnly;");
                await _localStorage.SetItemAsync("tokenExpiration", authResult.AccessTokenExpiration);

                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
            }

            return authResult;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity.IsAuthenticated;
        }
    }
}
