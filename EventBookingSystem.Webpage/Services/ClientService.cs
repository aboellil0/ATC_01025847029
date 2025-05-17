//using EventBookingSystem.Core.DTOs.Auth;
//using Intersoft.Crosslight.Mobile;
//using Microsoft.AspNetCore.Components.Authorization;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;

//namespace EventBookingSystem.Webpage.Services
//{
//    public class ClientService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly AuthenticationStateProvider _authStateProvider; // Use a custom provider  
//        private readonly ILocalStorageService _localStorage;
//        public ClientService(AuthenticationStateProvider authStateProvider, HttpClient httpClient, ILocalStorageService localStorage)
//        {
//            _authStateProvider = authStateProvider;
//            _httpClient = httpClient;
//            _localStorage = localStorage;
//        }
//        public async Task<bool> RegisterClientAsync(RegisterReq registerDto)
//        {
//            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
//            if (response.IsSuccessStatusCode)
//            {
//                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
//                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResponse.Token);
//                AccessToken = authResponse.Token;
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> LoginClientAsync(LoginReq loginReq)
//        {
//            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginReq);
//            if (response.IsSuccessStatusCode)
//            {
//                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
//                await _localStorage.SetItemAsync("authToken", result.Token);
//                AccessToken = authResponse.Token;
//                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResponse.Token);
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public async Task<bool> LogoutClientAsync()
//        {
//            var response = await _httpClient.PostAsync("api/auth/logout", null);
//            if (response.IsSuccessStatusCode)
//            {
//                AccessToken = null;
//                _httpClient.DefaultRequestHeaders.Authorization = null;
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }
//}
